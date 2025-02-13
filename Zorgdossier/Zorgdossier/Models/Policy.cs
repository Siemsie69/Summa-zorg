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
using System.ComponentModel;

namespace Zorgdossier.Models
{
    [Table("Policy")]
    public class Policy : ObservableObject
    {
        #region Fields
        private int _dossierId;
        private string _urgency = string.Empty;
        private string? _triageCriteria = string.Empty;
        private string _policyChoice = string.Empty;
        private DateTime? _policyDateTime;
        #endregion

        #region Properties
        [Key]
        public int Id { get; set; }

        [Required]
        public int DossierId
        {
            get { return _dossierId; }
            set { _dossierId = value; OnPropertyChanged(); }
        }

        [StringLength(25), Required]
        public string Urgency
        {
            get { return _urgency; }
            set { _urgency = value; OnPropertyChanged(); }
        }

        [StringLength(320)]
        public string? TriageCriteria
        {
            get { return _triageCriteria; }
            set { _triageCriteria = value; OnPropertyChanged(); }
        }

        [StringLength(25), Required]
        public string PolicyChoice
        {
            get { return _policyChoice; }
            set { _policyChoice = value; OnPropertyChanged(); }
        }

        public DateTime? PolicyDateTime
        {
            get { return _policyDateTime; }
            set { _policyDateTime = value; OnPropertyChanged(); }
        }
        #endregion

        #region Relations
        [ForeignKey(nameof(DossierId))]
        public Dossier Dossier
        {
            get; set;
        }
        #endregion
    }
}