using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace OnATheme
{
    public partial class FormAddBlockVariant : Form
    {
        List<TextureGroup> TextureGroups = new List<TextureGroup>();
        List<string> TGTextures = new List<string>();
        List<string> TGFaces = new List<string>();
        List<Texture> SeqConst = new List<Texture>();
        List<Texture> SeqVar = new List<Texture>();

        public FormAddBlockVariant()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Show the dialogue, and return an Attribute object
        /// </summary>
        /// <returns></returns>
        public static BlockVariant ShowAndReturnObject(string BlockName)
        {
            // Create the dialog
            FormAddBlockVariant dialog = new FormAddBlockVariant();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                // Parse checkboxes into rotation arrays
                bool[] xRot = new bool[4] { dialog.checkBoxX0.Checked, dialog.checkBoxX90.Checked, dialog.checkBoxX180.Checked, dialog.checkBoxX270.Checked };
                bool[] yRot = new bool[4] { dialog.checkBoxY0.Checked, dialog.checkBoxY90.Checked, dialog.checkBoxY180.Checked, dialog.checkBoxY270.Checked };

                // Use Use overwrite text fro name, otherwise use parent
                string modelName  = dialog.textBoxParentModel.Text;
                if (dialog.textBoxOverwrite.Text != "")
                    modelName  = dialog.textBoxOverwrite.Text;

                Model m;

                // Create the model
                if (dialog.radioButtonTypeExponential.Checked)
                    m = new ModelCompoundExponential(modelName, dialog.textBoxParentModel.Text, dialog.TextureGroups, xRot, yRot);
                else
                    m = new ModelCompoundSequential(modelName, dialog.textBoxParentModel.Text, dialog.SeqConst, dialog.SeqVar, (int)dialog.numericUpDownNumVariants.Value, xRot, yRot);

                // Create the block variant and return
                BlockVariant newBlock = new BlockVariant(dialog.textBoxName.Text, m);
                return newBlock;
            }
            else
            {
                return null; // In case of canceled dialogue. Make sure to handle this appropriately.
            }
        }
        /// <summary>
        /// Add a texture to be added to the texture group
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonAddTexture_Click(object sender, EventArgs e)
        {
            TGTextures.Add(Texture.TEXTURE_PATH + textBoxTexture.Text);
            listBoxTextures.Items.Add(TGTextures[TGTextures.Count - 1]);
        }
        /// <summary>
        /// Add a reference to be added to the faces for a texture group
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonAddRef_Click(object sender, EventArgs e)
        {
            TGFaces.Add(textBoxRef.Text);
            listBoxFaces.Items.Add(TGFaces[TGFaces.Count - 1]);
        }
        /// <summary>
        /// Open the github.io page.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void linkLabelInstructions_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://git.secretonline.co/OnAThemeJSONGen/");
        }
        /// <summary>
        /// Turn the strings into a texture group
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonAddTextureGroup_Click(object sender, EventArgs e)
        {
            if (TGFaces.Count > 0 && TGTextures.Count > 0)
            {
                TextureGroups.Add(new TextureGroup(TGFaces, TGTextures));
                listBoxTextureGroups.Items.Add(TextureGroups[TextureGroups.Count - 1]);
                listBoxFaces.Items.Clear();
                listBoxTextures.Items.Clear();
                // Change list refernces to new list, rather than the old one
                TGFaces = new List<string>();
                TGTextures = new List<string>();
            }
        }
        /// <summary>
        /// Model type changed to sequential
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButtonTypeSequential_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonTypeSequential.Checked)
            {
                groupBoxTextureGroups.Enabled = false;
                groupBoxTexturesSequential.Enabled = true;
                buttonCreate.Enabled = true;
            }
        }
        /// <summary>
        /// Model type changed to exponential
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButtonTypeExponential_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonTypeExponential.Checked)
            {
                groupBoxTextureGroups.Enabled = true;
                groupBoxTexturesSequential.Enabled = false;
                buttonCreate.Enabled = true;
            }
        }
        /// <summary>
        /// Add a constant texture to the current sequential model
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonSeqConstAdd_Click(object sender, EventArgs e)
        {
            SeqConst.Add(new Texture(textBoxSeqConstReference.Text, textBoxSeqConstTexture.Text));
            listBoxSeqConst.Items.Add(SeqConst[SeqConst.Count - 1]);
        }
        /// <summary>
        /// Add a variable texture to the durrent sequential model
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonSeqVarAdd_Click(object sender, EventArgs e)
        {
            SeqVar.Add(new Texture(textBoxSeqVarReference.Text, textBoxSeqVarTexture.Text));
            listBoxSeqVar.Items.Add(SeqVar[SeqVar.Count - 1]);
        }
    }
}
