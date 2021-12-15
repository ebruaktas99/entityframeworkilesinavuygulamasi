using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SinavE
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        SinavEEntities db = new SinavEEntities();
        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void LinqEntity_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked == true)
            {
                var degerler = db.NOTLAR.Where(x => x.SINAV1 < 50);
                dataGridView1.DataSource = degerler.ToList();
            }
            if (radioButton2.Checked == true)
            {
                var degerler = db.OGRENCILER.Where(x => x.OGRENCIAD =="Ali");
                dataGridView1.DataSource = degerler.ToList();
            }

            if (radioButton3.Checked == true)
            {
                var degerler = db.OGRENCILER.Where(x => x.OGRENCIAD == textBox1.Text || x.OGRENCISOYAD == textBox1.Text);
                dataGridView1.DataSource = degerler.ToList();
            }
            //sadece soyad
            if (radioButton4.Checked == true)
            {
                var degerler = db.OGRENCILER.Select(x => new { soyadi = x.OGRENCISOYAD });
                dataGridView1.DataSource = degerler.ToList();
            }

            if (radioButton5.Checked == true)
            {
                var degerler = db.OGRENCILER.Select(x => new { Ad = x.OGRENCIAD.ToUpper(), Soyad=x.OGRENCISOYAD.ToLower()});
                dataGridView1.DataSource = degerler.ToList();
            }

            //Şartlı Seçim
            if (radioButton6.Checked == true)
            {
                var degerler = db.OGRENCILER.Select(x => new { Ad = x.OGRENCIAD.ToUpper(), Soyad = x.OGRENCISOYAD.ToLower() }).Where(x => x.Ad !="Ali");
                dataGridView1.DataSource = degerler.ToList();
            }

            //Geçti mi 
            if (radioButton7.Checked == true)
            {
                var degerler = db.NOTLAR.Select(X =>
                new
                {
                    OgrenciAd = X.OGRENCIID,
                    Ortalaması = X.ORTALAMA,
                    Durumu = X.DURUM == true ? "Geçti" : "Kaldı"
                });
                dataGridView1.DataSource = degerler.ToList();
            }

            //Birleştirme işlemi : Ad ve ortalama gelir.
            if (radioButton8.Checked == true)
            {
                var degerler = db.NOTLAR.SelectMany(x => db.OGRENCILER.Where(y => y.OGRENCIID == x.OGRENCIID) , (x,y) => new
                {
                    y.OGRENCIAD,
                    x.ORTALAMA
            });
                dataGridView1.DataSource = degerler.ToList();
            }
            
            //İlk 3 değer

            if (radioButton9.Checked == true)
            {
                var degerler = db.OGRENCILER.OrderBy(x => x.OGRENCIID).Take(3);
                dataGridView1.DataSource = degerler.ToList();
            }
            
            //Son 3 değer
            if (radioButton10.Checked == true)
            {
                var degerler = db.OGRENCILER.OrderByDescending(x => x.OGRENCIID).Take(3);
                dataGridView1.DataSource = degerler.ToList();
            }

            //Ada göre sırala 
            if (radioButton11.Checked == true)
            {
                var degerler = db.OGRENCILER.OrderBy(x => x.OGRENCIAD);
                dataGridView1.DataSource = degerler.ToList();
            }

            //5 tane atla
            if (radioButton12.Checked == true)
            {
                var degerler = db.OGRENCILER.OrderBy(x => x.OGRENCIID).Skip(5);
                dataGridView1.DataSource = degerler.ToList();
            }


        }
    }
}
