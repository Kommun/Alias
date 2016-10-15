using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alias.Model
{
    public class ShopItem
    {
        /// <summary>
        /// Название продукта
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Форматированная цена
        /// </summary>
        public string FormattedPrice { get; set; }

        /// <summary>
        /// ID продукта
        /// </summary>
        public string ProductId { get; set; }

        /// <summary>
        /// Куплен ли товар
        /// </summary>
        public bool IsBought { get; set; }

        /// <summary>
        /// Отображаемая цена
        /// </summary>
        public string Price
        {
            get { return IsBought ? "Куплено" : FormattedPrice; }
        }

        /// <summary>
        /// Доступна ли покупка
        /// </summary>
        public bool IsEnabled
        {
            get { return !IsBought; }
        }
    }
}
