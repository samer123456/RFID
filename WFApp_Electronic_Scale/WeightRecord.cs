using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace WFApp_Electronic_Scale
{
    public class WeightRecord1
    {
        //public long No { get; set; }
        //public int Id { get; set; }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Kart { get; set; }
        public string Plaka { get; set; }
        public int Tartim1 { get; set; }
        public DateTime? Tarih1 { get; set; }
        public string Saat1 { get; set; }
        public string Sorgu1 { get; set; }
        public string Sorgu2 { get; set; }
        public string Sorgu3 { get; set; }
        public string Sorgu4 { get; set; }
        public string Sorgu5 { get; set; }
        public string Kullanici1 { get; set; }
        public string Aciklama1 { get; set; }
        public string Kantar1 { get; set; }
        public string Istiap { get; set; }
        public string Sorgu6 { get; set; }
    }
    public class WeightRecord2
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long No { get; set; }
        //public int Id { get; set; }
        public string Kart { get; set; }
        public string Plaka { get; set; }
        public int? Tartim2 { get; set; }
        public int? Tartim1 { get; set; }
        public int? Net { get; set; }
        public DateTime? Tarih2 { get; set; }
        public string Saat2 { get; set; }
        public DateTime? Tarih1 { get; set; }
        public string Saat1 { get; set; }
        public string Sorgu1 { get; set; }
        public string Sorgu2 { get; set; }
        public string Sorgu3 { get; set; }
        public string Sorgu4 { get; set; }
        public string Sorgu5 { get; set; }
        public string Kullanici2 { get; set; }
        public string Kullanici1 { get; set; }
        public string Aciklama2 { get; set; }
        public string Aciklama1 { get; set; }
        public string Kantar1 { get; set; }
        public string Kantar2 { get; set; }
        public string Sure { get; set; }
        public string Periyod { get; set; }
        public string Istiap { get; set; }
        public string Sorgu6 { get; set; }
    }
}


