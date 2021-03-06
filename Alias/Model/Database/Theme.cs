﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace Alias.Model.Database
{
    public class Theme
    {
        /// <summary>
        /// ID темы
        /// </summary>
        [PrimaryKey, AutoIncrement]
        public int ThemeId { get; set; }

        /// <summary>
        /// ID набора (магазинного)
        /// </summary>
        public int PackId { get; set; }

        /// <summary>
        /// Название темы
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public string Description { get; set; }
    }
}
