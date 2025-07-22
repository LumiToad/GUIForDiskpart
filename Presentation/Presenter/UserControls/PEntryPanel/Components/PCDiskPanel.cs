using System;
using System.Collections.Generic;

using GUIForDiskpart.Presentation.View.UserControls;


namespace GUIForDiskpart.Presentation.Presenter.UserControls
{
    public class PCDiskPanel
    {
        UCEntryPanel userControl;
        PEntryPanel presenter;

        private Dictionary<UCPhysicalDriveEntry, PPhysicalDriveEntry> Disk_K_uc_V_p = new();

        UInt32? previousSelected;

        public void Setup(UCEntryPanel userControl, PEntryPanel presenter)
        {
            this.userControl = userControl;
            this.presenter = presenter;
        }

        public void UpdatePanel(List<DiskModel> disks)
        {
            userControl.Stack.Children.Clear();
            Disk_K_uc_V_p.Clear();

            foreach (var disk in disks)
            {
                var ucPhysicalDrive = new UCPhysicalDriveEntry();
                var pPhysicalDrive = presenter.CreateUCPresenter<PPhysicalDriveEntry>(ucPhysicalDrive, disk);
                pPhysicalDrive.ESelected += OnSelected;
                Disk_K_uc_V_p.Add(ucPhysicalDrive, pPhysicalDrive);
                userControl.Stack.Children.Add(ucPhysicalDrive);
            }
        }

        public UInt32? GetSelectedDiskIdx()
        {
            foreach (UCPhysicalDriveEntry entry in userControl.Stack.Children)
            {
                var pPhysicalDrive = Disk_K_uc_V_p[entry];
                if (pPhysicalDrive != null && pPhysicalDrive.IsSelected == true)
                    return pPhysicalDrive.Disk.DiskIndex;
            }

            return null;
        }

        public UCPhysicalDriveEntry? GetSelectedEntry()
        {
            foreach (UCPhysicalDriveEntry entry in userControl.Stack.Children)
            {
                var pPhysicalDrive = Disk_K_uc_V_p[entry];
                if (pPhysicalDrive != null && pPhysicalDrive.IsSelected == true)
                    return entry;
            }

            return null;
        }

        public PPhysicalDriveEntry GetEntryPresenter(UCPhysicalDriveEntry entry)
        {
            return Disk_K_uc_V_p[entry];
        }

        public void SelectPrevious()
        {
            foreach (UCPhysicalDriveEntry entry in userControl.Stack.Children)
            {
                var pPhysicalDrive = Disk_K_uc_V_p[entry];
                if (
                    pPhysicalDrive != null &&
                    pPhysicalDrive.Disk != null &&
                    pPhysicalDrive.IsSelected != true &&
                    pPhysicalDrive.Disk.DiskIndex == previousSelected
                    )
                {
                    pPhysicalDrive.Select();
                    return;
                }
            }

            var first = Disk_K_uc_V_p[userControl.Stack.Children[0] as UCPhysicalDriveEntry];
            first.Select();
        }

        public void OnSelected()
        {
            UInt32? idx = GetSelectedDiskIdx();
            if (idx != null)
                previousSelected = idx;
        }
    }
}
