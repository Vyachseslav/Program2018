using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kachatel2018.ViewModel
{
    class BaseViewModel : BindableBase
    {
        private RelayCommand _navigator;

        private static IntroduceViewModel _introduceVM = new IntroduceViewModel();
        private static WhatDoViewModel _whatDoVM = new WhatDoViewModel();
        private static GetExcelFilesViewModel _GetExcelVM = new GetExcelFilesViewModel();
        private static FirstCheckDataViewModel _firstCheckData = new FirstCheckDataViewModel();
        private static Merge_ExcelViewModel _mergeExcel = new Merge_ExcelViewModel();
        private static PreparationForLoadDBViewModel _preparation = new PreparationForLoadDBViewModel();        
        private static ConvertationIntoDatabaseViewModel _convertation = new ConvertationIntoDatabaseViewModel();
        private static LastStepAfterLoadingViewModel _lastStep = new LastStepAfterLoadingViewModel();
        private static DoMergeViewModel _doMerge = new DoMergeViewModel();

        // Дополнительные описания.
        private static TabRazrabViewModel _tabRazrabVM = new TabRazrabViewModel();
        private static FastInsertPassportIdIntoExcelViewModel _fastIdPassportVM = new FastInsertPassportIdIntoExcelViewModel();
        private static RecoveryBackupIntoMETR6ViewModel _recoverVM = new RecoveryBackupIntoMETR6ViewModel();
        private static CreateBackupDatabaseViewModel _createBackup = new CreateBackupDatabaseViewModel();


        private BindableBase _currentViewModel;
        public BindableBase CurrentViewModel
        {
            get { return _currentViewModel; }
            set { SetProperty(ref _currentViewModel, value); }
        }

        public BaseViewModel()
        {
            CurrentViewModel = _introduceVM;
        }

        public RelayCommand NavigationCommand
        {
            get
            {
                return _navigator ??
                    (_navigator = new RelayCommand(obj =>
                    {
                        Navigation((string)obj);
                    }));
            }
        }


        private void Navigation(string viewModelName)
        {
            switch (viewModelName)
            {
                case "Introduce":
                    CurrentViewModel = _introduceVM;
                    break;
                case "WhatDo":
                    CurrentViewModel = _whatDoVM;
                    break;
                case "GetExcelFiles":
                    CurrentViewModel = _GetExcelVM;
                    break;
                case "FirstCheckData":
                    CurrentViewModel = _firstCheckData;
                    break;
                case "Merge_Excel":
                    CurrentViewModel = _mergeExcel;
                    break;
                case "PreparationForLoadDB":
                    CurrentViewModel = _preparation;
                    break;
                case "ConvertationIntoDatabase":
                    CurrentViewModel = _convertation;
                    break;
                case "LastStepAfterLoading":
                    CurrentViewModel = _lastStep;
                    break;
                case "DoMerge":
                    CurrentViewModel = _doMerge;
                    break;
                case "TabRazrab":
                    CurrentViewModel = _tabRazrabVM;
                    break;
                case "FastInsertPassportIdIntoExcel":
                    CurrentViewModel = _fastIdPassportVM;
                    break;
                case "RecoveryBackupIntoMETR6":
                    CurrentViewModel = _recoverVM;
                    break;
                case "CreateBackupDatabase":
                    CurrentViewModel = _createBackup;
                    break;
            }
        }
    }
}
