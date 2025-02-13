using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Zorgdossier.Helpers;
using Zorgdossier.Views.SectieViews;

namespace Zorgdossier.Models
{
    [Table("Student")]
    public class Student : ObservableObject
    {
        #region Fields
        private string _studentNumber = string.Empty;
        private string _name = string.Empty;
        private string _deviceName = string.Empty;
        private DateOnly _createdAt;
        #endregion

        #region Properties
        [Key]
        public int Id
        {
            get; set;
        }

        [StringLength(8), Required]
        public string StudentNumber
        {
            get
            {
                return _studentNumber;
            }
            set
            {
                _studentNumber = value; OnPropertyChanged();
            }
        }

        [StringLength(50), Required]
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value; OnPropertyChanged();
            }
        }

        [StringLength(15), Required]
        public string DeviceName
        {
            get
            {
                return _deviceName;
            }
            set
            {
                _deviceName = value; OnPropertyChanged();
            }
        }

        [Required]
        public DateOnly CreatedAt
        {
            get
            {
                return _createdAt;
            }
            set
            {
                _createdAt = value; OnPropertyChanged();
            }
        }
        #endregion

        #region Relations
        public ICollection<Dossier> Dossiers
        {
            get; set;
        } = new List<Dossier>();
        #endregion
    }
}