using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;
using blueapp.Data;
using blueapp.Models;
using blueapp.Resources.Localization;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Views;
using MvvmHelpers;
using MvvmHelpers.Commands;

namespace blueapp.ViewModels
{
    public class GraphViewModel : BaseViewModel
    {
        // 리프래쉬 커맨드 필요시 사용
        public AsyncCommand RefreshCommand { get; }
        private DatabaseService _databaseService;
        public GraphDrawable GraphDrawable { get; private set; }
        public ObservableCollection<OperationRecord> GraphRecords { get; private set; }

        public double GraphWidth => GraphRecords.Count * 50; // 너비를 조정할 속성
        private bool _isRefreshing;

        public GraphViewModel()
        {
            RefreshCommand = new AsyncCommand(RefreshGraph);
            _databaseService = new DatabaseService();
            GraphDrawable = new GraphDrawable();
            GraphRecords = new ObservableCollection<OperationRecord>();
        }

        #region 리프래쉬 코드
        public bool IsRefreshing
        {
            get => _isRefreshing;
            set => SetProperty(ref _isRefreshing, value);
        }

        public async Task RefreshGraph()
        {
            try
            {
                // 그래프 업데이트
                await UpdateGraphRecords();
            }
            catch (TaskCanceledException ex)
            {
                // TaskCanceledException 처리
                Debug.WriteLine($"Task was canceled: {ex.Message}");
            }
            catch (Exception ex)
            {
                // 일반 예외 처리
                Debug.WriteLine($"An error occurred: {ex.Message}");
            }
            finally
            {
                IsRefreshing = false;
            }
        }
        #endregion
        
        #region 그래프 업데이트
        public async Task UpdateGraphRecords()
        {
            // 그래프 초기화
            GraphRecords.Clear();

            // 최근 데이터의 0번째부터 20개 출력
            var recordList = await _databaseService.GetRecordsAsync(0, 20);

            // 데이터를 반대로 정렬하여 최신 데이터가 오른쪽에 위치하도록 함
            var latestRecords = recordList.OrderBy(r => r.Timestamp);
            foreach (var record in latestRecords)
            {
                GraphRecords.Add(record);
            }
            DrawGraph();
        }

        public void DrawGraph()
        {
            // 그래프 그리기
            GraphDrawable.Records = new List<OperationRecord>(GraphRecords);
            // 그래프 넓이 변경 업데이트
            OnPropertyChanged(nameof(GraphWidth));
        }
        #endregion
    }
}
