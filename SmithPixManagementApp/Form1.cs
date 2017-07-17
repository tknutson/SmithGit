using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SmithPixManagementApp
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// Add/Edit Pics
        ///     Show all Pics if Editing.
        ///         User clicks on pic, brings up edit screen.
        ///     If Adding, run a wizard.  Add new pic info.  Allow
        ///         option to add new pic to one or more categories.
        /// Add/Edit Categories
        ///     Show all Cats if Editing.
        ///         User clicks on Cat.  Bring up edit screen.
        ///     If Adding, run a wizard.  Add new Category info.  Add
        ///         new category to Front Page.  Select pic from Cat
        ///         to be on Front Page.
        /// </summary>
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'photosSmithPixDataSet.UpdatePhotoInfoView' table. You can move, or remove it, as needed.


            //TODO: Create a view to show all data a user needs per Cat, per Pic.  Then, pull in thumbs
            //  Pic name, Category name, pic description, pic order, inactive switch
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            try
            {
                this.Validate();

            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }
    }
}
