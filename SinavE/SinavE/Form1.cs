using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace SinavE
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        SinavEEntities db = new SinavEEntities();
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnDersListesi_Click(object sender, EventArgs e)
        {
            SqlConnection baglanti = new SqlConnection(@"Data Source=DESKTOP-81USL0I\SQLEXPRESS;Initial Catalog=SinavE;Integrated Security=True");
            SqlCommand komut = new SqlCommand("Select * From DERSLER",baglanti);
            SqlDataAdapter da = new SqlDataAdapter(komut); //Bağlayıcı oluşturuyoruz.
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;

        }

        private void btnOgrListele_Click(object sender, EventArgs e)
        {
          
            dataGridView1.DataSource = db.OGRENCILER.ToList();
            // Listelenecek değerleri kendimiz belirliyoruz. 1.yol
            dataGridView1.Columns[3].Visible = false; //fotoğraf alanı görünmesin
            dataGridView1.Columns[4].Visible = false; //notlar görünmesin.
        }

        private void btnNot_Click(object sender, EventArgs e)
        {
            //Linq Sorgu Ve Anonymus Type : Listelenecek değerleri kendimiz belirliyoruz. 2.yol
            //item değişken
            //select new : yeni parantez içindekileri seç, item'a bağlı değişkenden tablolar çekilir.
            var query = from item in db.NOTLAR
                        select new { item.NOTID, item.OGRENCILER.OGRENCIAD,item.OGRENCILER.OGRENCISOYAD,item.DERSLER.DERSAD, item.SINAV1};


             dataGridView1.DataSource= query.ToList(); 
         }

        private void btnKaydet_Click(object sender, EventArgs e)
        {
            //yeni öğrenci ekleme işlemi

            OGRENCILER t = new OGRENCILER();
            //t nesnei aracılığıyla ogrencilerdeki proplara ulaşıyoruz.

            t.OGRENCIAD = txtAd.Text;
            t.OGRENCISOYAD = txtSoyad.Text;

            db.OGRENCILER.Add(t);
            db.SaveChanges();//değişiklikleri veritabına kaydet.
            MessageBox.Show("Öğrenci Listeye Eklenmiştir.");
        }

        private void btnSil_Click(object sender, EventArgs e)
        {

            int id = Convert.ToInt32(txtId.Text); //ID textboxına girilen değer idye atanır.
            var x = db.OGRENCILER.Find(id); //silinmek istenen değeri kullanıcıdan alıyoruz. İd den alına değere göre bulunur.

            db.OGRENCILER.Remove(x);
            db.SaveChanges();
            MessageBox.Show("Öğrenci Listeden Silinmiştir.");

        }

        private void btnGuncelle_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(txtId.Text);
            var x = db.OGRENCILER.Find(id);

            x.OGRENCIAD = txtAd.Text;
            x.OGRENCISOYAD = txtSoyad.Text;
            x.OGRENCIFOTO = txtFoto.Text;

            db.SaveChanges();
            MessageBox.Show("Öğrenci Bilgileri Güncellenmiştir.");
        }

        //Update Model From Database : Veritabanında yaptığımız değişiklikleri visual studioya yansıtabilmek için kullanılır.

        /*PROSEDÜR OLUŞTURMA:
        sql de oluşturulan prosedürü update modelden vs ye eklemek gerekir.

        Türkçe karşılığı olarak saklı yordam veya alt yordam olarak geçer.Stored Procedure’ler veritabanın da saklanan SQL ifadeleridir, parametrelerle çalışabilirler.Programlama dillerinde ki fonksiyonlar gibi düşünebiliriz, ihtiyacımız olduğunda çağırıp kullanabileceğimiz, başlangıçta derlendiği için normal SQL sorgularından daha performanslı olarak çalışabilirler.

        Store Procedure Özellikleri

        Performansı arttırır.
        Güvenliği arttırır
        Bir kez yazıp birçok yerde kullanabiliriz
        Ağ trafiğini yormadan çalışırlar*/

        private void btnProsedur_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = db.NOTLISTESI();  //Prosedürü çağırmış olduk.    
        }


        /*Lambda İfadeler : Sadeleştirilmiş ananim fonksiyonlardır. Atama işlemi yapılır.*/


        private void btnBul_Click(object sender, EventArgs e)
        {
            //Lambda İfade ile arama
            dataGridView1.DataSource = db.OGRENCILER.Where(x => x.OGRENCIAD == txtAd.Text).ToList(); //txt ye girilen değeri bulur.
        }

        /*Linq Sorgular Ve Contains Metodu İle Arama İşlemi
         
         TextBoxa girilen harfe göre listeleme yapılır.*/

        private void txtAd_TextChanged(object sender, EventArgs e)
        {
            string aranan = txtAd.Text;

            var degerler = from item in db.OGRENCILER

                           where item.OGRENCIAD.Contains(aranan)
                           
                           select item;

            dataGridView1.DataSource = degerler.ToList();


        }

        //ÖRNEK LINQ SORGULARI
        private void btnEntity_Click(object sender, EventArgs e)
        {
            //A - Z
            if (radioButton1.Checked== true)
            {
                List<OGRENCILER> liste1 = db.OGRENCILER.OrderBy(p => p.OGRENCIAD).ToList();
                dataGridView1.DataSource = liste1;
            }

            // Z - A

            if (radioButton2.Checked == true)
            {
                List<OGRENCILER> liste2 = db.OGRENCILER.OrderByDescending(p => p.OGRENCIAD).ToList();
                dataGridView1.DataSource = liste2;
            }

            //İlk üç tanesini sırala

            if (radioButton3.Checked == true)
            {
                List<OGRENCILER> liste3 = db.OGRENCILER.OrderBy(p => p.OGRENCIAD).Take(3).ToList();
                dataGridView1.DataSource = liste3;
            }

            // ID ye göre değer getirir.
            if (radioButton4.Checked == true)
            {
                List<OGRENCILER> liste4 = db.OGRENCILER.Where(p => p.OGRENCIID == 5).ToList();
                dataGridView1.DataSource = liste4;
            }

            //A harfi ile başlayanlar
            if (radioButton5.Checked == true)
            {
                List<OGRENCILER> liste5 = db.OGRENCILER.Where(p => p.OGRENCIAD.StartsWith("a")).ToList();
                dataGridView1.DataSource = liste5;
            }

            //A harfi ile bitenlerr
            if (radioButton6.Checked == true)
            {
                List<OGRENCILER> liste6 = db.OGRENCILER.Where(p => p.OGRENCIAD.EndsWith("a")).ToList();
                dataGridView1.DataSource = liste6;
            }

            //Tabloda değer var mı ?
            if (radioButton7.Checked == true)
            {
                bool deger = db.OGRENCILER.Any();
                MessageBox.Show(deger.ToString(), "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            //Toplam öğrenci sayısı
            if (radioButton8.Checked == true)
            {
                int toplam = db.OGRENCILER.Count();
                MessageBox.Show(toplam.ToString(), "Toplam Öğrenci Sayısı", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            //Sınav1 Toplam Puan
            if (radioButton9.Checked == true)
            {
                var toplam = db.NOTLAR.Sum(p => p.SINAV1);
                MessageBox.Show("Toplam Sınav1 Puanı :",toplam.ToString());
            }

            //Sınav1 Ortalama Puanı
            if (radioButton10.Checked == true)
            {
                var toplam = db.NOTLAR.Average(p => p.SINAV1);
                MessageBox.Show("Sınav1'in Ortalaması :", toplam.ToString());
            }

            //Sınav1 Puanı Ortalamadan Yüksek Olan Öğrenciler
            if (radioButton11.Checked == true)
            {
                var ORT = db.NOTLAR.Average(p => p.SINAV1);
                List<NOTLAR> liste7 = db.NOTLAR.Where(p => p.SINAV1 >= ORT).ToList();
                dataGridView1.DataSource = liste7;
            }

            //max sınav1 notu
            if (radioButton10.Checked == true)
            {
                var enyuksek= db.NOTLAR.Max(p => p.SINAV1);
                MessageBox.Show("Sınav1'in En Yüksek Alınan Puanı:", enyuksek.ToString());
            }

            //min sınav1 notu
            if (radioButton13.Checked == true)
            {
                var endusuk = db.NOTLAR.Min(p => p.SINAV1);
                MessageBox.Show("Sınav1'in En Yüksek Alınan Puanı:", endusuk.ToString());
            }

            //SInav1 en yüksek notu alan öğrenci
            if (radioButton14.Checked == true)
            {
                var yuksek = db.NOTLAR.Max(p => p.SINAV1);
                var sorgu = from item in db.NOTLISTESI().Where(p => p.SINAV1== yuksek)
                            select new { item.AD_SOYAD };

                dataGridView1.DataSource = sorgu.ToList();
            }
        }

        private void btnJoin_Click(object sender, EventArgs e)
        {
            //notlar ve ogrenciler tablolarını birleştir.
            var sorgu = from d1 in db.NOTLAR

                        join d2 in db.OGRENCILER

                           on d1.OGRENCIID equals d2.OGRENCIID //değerler eşit olmalı

                        join d3 in db.DERSLER

                        on d1.DERSID equals d3.DERSID

                     

                        select new
                        {
                            //Sol taraftakiler datagridviewda tabloda yazacak alan adıdır. Kendimiz belirliyoruz.
                            ÖĞRENCİ = d2.OGRENCIAD + " " + d2.OGRENCISOYAD,
                            //SOYAD = d2.OGRENCISOYAD,
                            DERS  = d3.DERSAD,
                            SINAV1 = d1.SINAV1,
                            SINAV2 = d1.SINAV2,
                            SINAV3 = d1.SINAV3,
                            ORTALAMA = d1.ORTALAMA
                        };

            dataGridView1.DataSource = sorgu.ToList();


        }
    }
}
