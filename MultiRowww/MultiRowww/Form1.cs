using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MultiRowww
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// 元データ
        /// </summary>
        public class sData
        {
            public int key { get; set; }
            public int subkey1 { get; set; }
            public int subkey2 { get; set; }
            public string moji1 { get; set; }
            public string moji2 { get; set; }
        }
        /// <summary>
        /// 表示用データ
        /// </summary>
        public class sDisp
        {
            public int key { get; set; }            //隠しキー
            public string dispkey { get; set; }     //表示用キー
            public int subkey { get; set; }
            public string moji { get; set; }
        }


        //dataGrid用設定
        private DataGridViewCellStyle defaultCellStyle;     //デフォルトのセルスタイル
        private DataGridViewCellStyle mouseCellStyle;       //マウスポインタの下にあるセルのセルスタイル



        private List<sData> lData = new List<sData>();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //テストデータ作成

            for (int i = 0; i < 300; i++)
            {
                sData wData = new sData();
                wData.key = i;
                wData.subkey1 = i + 100;
                wData.subkey2 = i + 200;
                wData.moji1 = (i + 100).ToString() + "ほげ";
                wData.moji2 = (i + 200).ToString() + "ほげ";

                lData.Add(wData);
            }
            //元データをそのまま表示
            dataGridView2.DataSource = lData;
            dataGridView2.AutoResizeColumns();

            // デフォルトのセルスタイルの設定
            this.defaultCellStyle = new DataGridViewCellStyle();
            //現在のセルのセルスタイルの設定
            this.mouseCellStyle = new DataGridViewCellStyle();
            this.mouseCellStyle.BackColor = Color.LightGreen;
            this.mouseCellStyle.SelectionBackColor = Color.Green;

            List<sDisp> lDisp = new List<sDisp>();
            sDisp wDisp;
            foreach (sData wData in lData)
            {
                //１行を２行に変換する
                wDisp = new sDisp();
                wDisp.key = wData.key;
                wDisp.dispkey = wData.key.ToString();  //表示用キーは１段目のみに表示する
                wDisp.subkey = wData.subkey1;
                wDisp.moji = wData.moji1;
                lDisp.Add(wDisp);

                wDisp = new sDisp();
                wDisp.key = wData.key;
                wDisp.subkey = wData.subkey2;
                wDisp.moji = wData.moji2;
                lDisp.Add(wDisp);
            }
            //分解後のデータを表示
            dataGridView1.DataSource = lDisp;
            //dataGridView1.Columns[0].Width = 10;
            dataGridView1.AutoResizeColumns();
            ColorAllReset();
        }


        //DataGridView1のCellEnterイベントハンドラ
        private void DataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            //ヘッダー以外のセル
            if (e.ColumnIndex >= 0 && e.RowIndex >= 0)
            {
                DataGridView dgv = (DataGridView)sender;

                //ペアとなるRow
                int pearRow = PearRowIndex(e.RowIndex);

                for (int i = 1; i < dgv.Columns.Count; i++)
                {
                    dgv[i, e.RowIndex].Style = this.mouseCellStyle;
                    if (pearRow >= 0) dgv[i, pearRow].Style = this.mouseCellStyle;
                }

                Text = PearRowIndex(e.RowIndex).ToString();
            }
        }

        //DataGridView1のCellLeaveイベントハンドラ
        private void DataGridView1_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            /*
            //ヘッダー以外のセル
            if (e.ColumnIndex >= 0 && e.RowIndex >= 0)
            {
                DataGridView dgv = (DataGridView)sender;
                for (int i = 1; i < dgv.Columns.Count; i++)
                {
                    dgv[i, e.RowIndex].Style = this.defaultCellStyle;
                }
            }
            */
            ColorAllReset();
        }


        //全部色リセット
        private void ColorAllReset()
        {
            for (int c = 1; c < dataGridView1.Columns.Count; c++)
            {
                for (int r = 0; r < dataGridView1.Rows.Count; r++)
                {
                    dataGridView1[c, r].Style = this.defaultCellStyle;
                }
            }
        }

        //入力RowIndexをもとに、ペアとなる行を検索してペアRowIndexを返す
        private int PearRowIndex(int rowIndex)
        {
            //ヘッダー以外のセル
            DataGridView dgv = dataGridView1;

            //選択行の先頭列に隠しキーがあるのでそれを取得する
            int wKey = (int)dgv[0, rowIndex].Value;

            //上から同一キーを探す。ただし入力パラメータは除く
            for (int r = 0; r < dgv.Rows.Count; r++)
            {
                if (r != rowIndex)
                {
                    if (wKey == (int)dgv[0, r].Value)
                    {
                        return r;
                    }
                }
            }

            return -1;
        }


    }
}
