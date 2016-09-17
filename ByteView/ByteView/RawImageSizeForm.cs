using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ByteView
{
    public partial class RawImageSizeForm : Form
    {
        public int ImageWidth
        {
            get
            {
                int result = 0;
                if (!int.TryParse(TextBoxWidth.Text, out result))
                {
                    MessageBox.Show("Invalid width.", "Invalid Width", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                return result;
            }
        }

        public int ImageHeight
        {
            get
            {
                int result = 0;
                if (!int.TryParse(TextBoxHeight.Text, out result))
                {
                    MessageBox.Show("Invalid height.", "Invalid Height", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                return result;
            }
        }

        public RawImageSizeForm()
        {
            InitializeComponent();
        }

        private void ButtonOK_Click(object sender, EventArgs e)
        {
			DialogResult = DialogResult.OK;
            base.Close();
        }
    }
}
