using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Zorgdossier.Helpers;

namespace Zorgdossier.Models
{
    [Table("Tests")]
    [Index(nameof(Text), IsUnique = true, Name = "UX_Text")]
    internal class UserMessage : ObservableObject
    {
        #region fields
        private string _text = string.Empty;
        #endregion

        #region properties
        [Key]
        public int Id
        {
            get; set;
        }

        [StringLength(255)]
        public string Text
        {
            get
            {
                return _text;
            }
            set
            {
                _text = value; OnPropertyChanged();
            }
        }
        #endregion
    }
}