using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

using GUIForDiskpart.Presentation.View.UserControls;

namespace GUIForDiskpart.Presentation.Presenter.UserControls.Components
{
    public class PCPartitionPanel
    {
        UCEntryPanel userControl;
        PEntryPanel presenter;

        private Dictionary<UCPartitionEntry, PPartitionEntry> Partitions_K_uc_V_p = new();
        private Dictionary<UCUnallocatedEntry, PUnallocatedEntry> Unallocated_K_uc_V_p = new();

        UInt32? previousSelected;
        bool wasUnallocatedSelected = false;

        public void Setup(UCEntryPanel userControl, PEntryPanel presenter)
        {
            this.userControl = userControl;
            this.presenter = presenter;
        }

        public void UpdatePanel(List<PartitionModel> partitions)
        {
            userControl.Stack.Children.Clear();
            Partitions_K_uc_V_p.Clear();

            foreach (var partition in partitions)
            {
                var ucPartition = new UCPartitionEntry();
                var pPartition = presenter.CreateUCPresenter<PPartitionEntry>(ucPartition, partition);
                pPartition.ESelected += OnPartitionSelected;
                Partitions_K_uc_V_p.Add(ucPartition, pPartition);
                userControl.Stack.Children.Add(ucPartition);
            }
        }

        public void UpdatePanel(PPhysicalDriveEntry pDisk)
        {
            if (pDisk.Disk.UnallocatedSpace > 0)
            {
                var ucUnallocated = new UCUnallocatedEntry();
                var pUnallocated = presenter.CreateUCPresenter<PUnallocatedEntry>(ucUnallocated, pDisk.Disk);
                pUnallocated.ESelected += OnUnallocatedSelected;
                userControl.Stack.Children.Add(ucUnallocated);
                Unallocated_K_uc_V_p.Add(ucUnallocated, pUnallocated);
            }
        }

        public UInt32? GetSelectedPartitionNr()
        {
            foreach (UCPartitionEntry entry in userControl.Stack.Children)
            {
                var pPartition = Partitions_K_uc_V_p[entry];
                if (pPartition != null && pPartition.IsSelected == true)
                    return pPartition.Partition.WSM.PartitionNumber;
            }

            return null;
        }

        public UserControl? GetSelectedEntry()
        {
            UserControl? retVal = null;

            foreach (UserControl entry in userControl.Stack.Children)
            {
                if (entry is UCUnallocatedEntry)
                {
                    var pUnallocated = Unallocated_K_uc_V_p[entry as UCUnallocatedEntry];
                    if (pUnallocated != null && pUnallocated.IsSelected == true)
                        retVal = entry;
                }
                else if (entry is UCPartitionEntry)
                {
                    var pPartition = Partitions_K_uc_V_p[entry as UCPartitionEntry];
                    if (pPartition != null && pPartition.IsSelected == true)
                        retVal = entry;
                }
            }

            return retVal;
        }

        public PPartitionEntry GetEntry(UCPartitionEntry entry)
        {
            return Partitions_K_uc_V_p[entry];
        }

        public PUnallocatedEntry? GetEntryPresenter(UCUnallocatedEntry entry)
        {
            if (Unallocated_K_uc_V_p.ContainsKey(entry))
                return Unallocated_K_uc_V_p[entry];
            else return null;
        }

        public void SelectPrevious()
        {
            if (wasUnallocatedSelected)
            {
                foreach (var entry in userControl.Stack.Children)
                {
                    if (entry is UCPartitionEntry) continue;

                    var pUnallocated = GetEntryPresenter(new UCUnallocatedEntry());
                    if (pUnallocated != null)
                    {
                        pUnallocated.Select();
                    }
                    return;
                }
            }

            if (previousSelected != null) 
            {
                foreach (var entry in userControl.Stack.Children)
                {
                    if (entry is UCUnallocatedEntry) continue;

                    var pPartition = Partitions_K_uc_V_p[entry as UCPartitionEntry];
                    if (
                        pPartition != null &&
                        pPartition.Partition != null &&
                        pPartition.Partition.WSM != null &&
                        pPartition.Partition.WSM.PartitionNumber == previousSelected
                        )
                    {
                        pPartition.Select();
                        return;
                    }
                }
            }
        }

        private void OnPartitionSelected()
        {
            UInt32? idx = GetSelectedPartitionNr();
            if (idx != null)
            {
                previousSelected = idx;
                wasUnallocatedSelected = false;
            }
        }

        private void OnUnallocatedSelected()
        {
            wasUnallocatedSelected = true;
        }
    }
}
