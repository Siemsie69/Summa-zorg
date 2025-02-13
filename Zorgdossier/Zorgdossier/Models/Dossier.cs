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
using ZstdSharp.Unsafe;

namespace Zorgdossier.Models
{
    [Table("Dossier")]
    public class Dossier : ObservableObject
    {
        #region Fields
        private int _studentId;
        private string _title = string.Empty;
        private DateOnly _createdAt;
        private DateOnly _updatedAt;
        #endregion

        #region Properties
        [Key]
        public int Id { get; set; }

        [Required]
        public int StudentId
        {
            get {  return _studentId; }
            set { _studentId = value; OnPropertyChanged(); }
        }

        [StringLength(100), Required]
        public string Title
        {
            get { return _title; }
            set { _title = value; OnPropertyChanged(); }
        }

        [Required]
        public DateOnly CreatedAt
        {
            get { return _createdAt; }
            set { _createdAt = value; OnPropertyChanged(); }
        }

        [Required]
        public DateOnly UpdatedAt
        {
            get { return _updatedAt; }
            set { _updatedAt = value; OnPropertyChanged(); }
        }
        #endregion

        #region Relations
        [ForeignKey(nameof(StudentId))]
        public Student Student
        {
            get; set;
        }

        public BasicInformation BasicInformation
        {
            get; set;
        }

        public Phone Phone
        {
            get; set;
        }

        public Question Question
        {
            get; set;
        }

        public ComplaintsSymptoms ComplaintsSymptoms
        {
            get; set;
        }

        public Research Research
        {
            get; set;
        }

        public Policy Policy
        {
            get; set;
        }

        public ContactAdvice ContactAdvice
        {
            get; set;
        }

        public Treatment Treatment
        {
            get; set;
        }
        #endregion
    }
}