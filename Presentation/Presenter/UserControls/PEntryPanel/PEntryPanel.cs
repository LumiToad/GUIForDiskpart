global using PEntryPanel =
   GUIForDiskpart.Presentation.Presenter.UserControls.PEntryPanel<GUIForDiskpart.Presentation.View.UserControls.UCEntryPanel>;

using System;
using System.Collections.Generic;
using System.Windows.Controls;

using GUIForDiskpart.Presentation.Presenter.UserControls.Components;
using GUIForDiskpart.Presentation.View.UserControls;


namespace GUIForDiskpart.Presentation.Presenter.UserControls
{
    public class PEntryPanel<T> : UCPresenter<T> where T : UCEntryPanel
    {
        PCDiskPanel pcDiskPanel;
        PCPartitionPanel pcPartitionPanel;

        public void UpdatePanel(List<DiskModel> disks)
        {
            if (pcDiskPanel != null)
                pcDiskPanel.UpdatePanel(disks);
        }

        public void UpdatePanel(List<PartitionModel> partitions)
        {
            if (pcPartitionPanel != null)
                pcPartitionPanel.UpdatePanel(partitions);
        }

        public void UpdatePanel(PPhysicalDriveEntry pDisk)
        {
            if (pcPartitionPanel != null)
                pcPartitionPanel.UpdatePanel(pDisk);
        }

        public UInt32? GetSelectedIdx()
        {
            if (pcDiskPanel != null)
                return pcDiskPanel.GetSelectedDiskIdx();

            if (pcPartitionPanel != null)
                return pcPartitionPanel.GetSelectedPartitionNr();

            return null;
        }

        public dynamic GetSelectedEntry()
        {
            if (pcDiskPanel != null)
                return pcDiskPanel.GetSelectedEntry();

            if (pcPartitionPanel != null)        
                return pcPartitionPanel.GetSelectedEntry();

            return null;
        }

        public void SelectPrevious()
        {
            if (pcDiskPanel != null)
                pcDiskPanel.SelectPrevious();

            if (pcPartitionPanel != null)
                pcPartitionPanel.SelectPrevious();
        }

        public PPhysicalDriveEntry GetEntryPresenter(UCPhysicalDriveEntry entry) => pcDiskPanel.GetEntryPresenter(entry);
        public PPartitionEntry GetEntryPresenter(UCPartitionEntry entry) => pcPartitionPanel.GetEntry(entry);
        public PUnallocatedEntry? GetEntryPresenter(UCUnallocatedEntry entry) => pcPartitionPanel.GetEntryPresenter(entry);

        #region UCPresenter

        public override void AddCustomArgs(params object?[] args)
        {
            if (args[0] is PCDiskPanel)
            {
                pcDiskPanel = (PCDiskPanel)args[0];
            }
            else if (args[0] is PCPartitionPanel)
            {
                pcPartitionPanel = (PCPartitionPanel)args[0];
            }
        }

        public override void InitComponents()
        {
            if (pcDiskPanel != null)
            {
                pcDiskPanel.Setup(UserControl, this as PEntryPanel);
            }
            else if (pcPartitionPanel != null)
            {
                pcPartitionPanel.Setup(UserControl, this as PEntryPanel);
            }
        }

        #endregion UCPresenter
    }
}
