using blueapp.Models;
using blueapp.Service;
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
        public ObservableCollection<Product_Production_AdditemModel> Productions { get; }
        private bool isRefreshing;

        public ICommand RefreshCommand { get; }
        // public ICommand AddProductionCommand { get; }
        
        public ProductViewModel()
        {
            _productionService = new ProductionService(new HttpClient());
            Productions = new ObservableCollection<Product_Production_AdditemModel>();
            RefreshCommand = new Command(async () => await LoadProductions());
            // AddProductionCommand = new Command(async () => await AddProduction());
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
        public async Task<bool> AddProduction(string productname, int count)
        {
            var newProduction = new Product_Production_AdditemModel
            {
                ProductName = productname,
                ProductionDate = DateTime.Now,
                Quantity = count
            };

            if (await _productionService.AddItemAsync(newProduction))
            {
                await LoadProductions();
                return true; 
            }
            else
            {
                return false;
            }
        }
        #endregion
    }
}
