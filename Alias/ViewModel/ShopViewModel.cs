using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Store;
using Alias.Utils;
using Alias.Model;

namespace Alias.ViewModel
{
    public class ShopViewModel : PropertyChangedBase
    {
        private bool _isLoading;

        #region Properties

        /// <summary>
        /// Купить
        /// </summary>
        public RelayCommand BuyCommand { get; set; }

        /// <summary>
        /// Список товаров
        /// </summary>
        public List<ShopItem> ShopItems { get; set; }

        /// <summary>
        /// Загрузка
        /// </summary>
        public bool IsLoading
        {
            get { return _isLoading; }
            set
            {
                _isLoading = value;
                OnPropertyChanged("IsLoading");
            }
        }

        #endregion

        /// <summary>
        /// Конструктор
        /// </summary>
        public ShopViewModel()
        {
            BuyCommand = new RelayCommand(Buy);
            LoadList();
        }

        /// <summary>
        /// Загрузить список товаров
        /// </summary>
        private async void LoadList()
        {
            IsLoading = true;
            try
            {
                var products = await CurrentApp.LoadListingInformationByProductIdsAsync(new List<string> { "BigPack", "CelebritiesPack" });
                ShopItems = products.ProductListings.Values
                    .Select(p => new ShopItem
                    {
                        Name = p.Name,
                        Description = p.Description,
                        FormattedPrice = p.FormattedPrice,
                        ProductId = p.ProductId,
                        IsBought = CurrentApp.LicenseInformation.ProductLicenses[p.ProductId].IsActive
                    })
                    .OrderBy(si => si.IsBought)
                    .ToList();

                OnPropertyChanged("ShopItems");
            }
            catch { }
            IsLoading = false;
        }

        /// <summary>
        /// Купить
        /// </summary>
        private async void Buy(object parameter)
        {
            var product = parameter as ShopItem;

            try
            {
                var result = await CurrentApp.RequestProductPurchaseAsync(product.ProductId);
                if (result.Status != ProductPurchaseStatus.Succeeded)
                    return;

                product.IsBought = true;
                GameStat.Instance.FillThemesList();
            }
            catch { }
        }
    }
}
