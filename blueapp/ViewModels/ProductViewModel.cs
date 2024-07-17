using blueapp.Data;
using blueapp.Models;
using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace blueapp.ViewModels
{
    public class ProductViewModel : BaseViewModel
    {
        private readonly ProductionService _productionService;
        public ObservableCollection<ProductionModel> Productions { get; }
        private bool isRefreshing;

        public ICommand RefreshCommand { get; }
        public ICommand AddProductionCommand { get; }
        

        
        public ProductViewModel()
        {
            _productionService = new ProductionService(new HttpClient());
            Productions = new ObservableCollection<ProductionModel>();
            RefreshCommand = new Command(async () => await LoadProductions());
            AddProductionCommand = new Command(async () => await AddProduction());
        }

        #region 리프래쉬
        public bool IsRefreshing
        {
            get => isRefreshing;
            set => SetProperty(ref isRefreshing, value);
        }
        #endregion

        #region 제품 로드
        public async Task LoadProductions()
        {
            IsRefreshing = true;

            try
            {
                var productions = await _productionService.GetListAsync();
                Productions.Clear();

                foreach (var production in productions)
                {
                    Productions.Add(production);
                }
            }
            catch (Exception)
            {
                // Handle exceptions
            }
            finally
            {
                IsRefreshing = false;
            }
        }
        #endregion

        #region 제품 추가
        private async Task AddProduction()
        {
            var newProduction = new ProductionModel
            {
                ProductName = "New Product",
                ProductionDate = DateTime.Now,
                Quantity = 100
            };

            await _productionService.AddItemAsync(newProduction);
            await LoadProductions();
        }
        #endregion
    }
}
