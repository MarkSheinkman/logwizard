﻿/* 
 * Copyright (C) 2014-2016 John Torjo
 *
 * This file is part of LogWizard
 *
 * LogWizard is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * LogWizard is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 *
 * If you wish to use this code in a closed source application, please contact john.code@torjo.com 
 *
 * **** Get Latest version at https://github.com/jtorjo/logwizard **** 
*/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lw_common {
    public partial class select_color_form : Form {
        public bool copy_to_clipboard = false;

        public select_color_form(string title = "Select Color...") : this(title, Color.Black, Cursor.Position) {
        }

        public select_color_form(string title, Color preselect) : this(title, preselect, Cursor.Position) {
        }

        public select_color_form(string title, Color preselect, Point location) {
            InitializeComponent();
            this.title.Text = title;
            if ( preselect != util.transparent)
                picker.SelectedColor = preselect;
            Location = location;
        }

        public Color SelectedColor {
            get { return picker.SelectedColor; }
            set { picker.SelectedColor = value; }
        }

        private void ok_Click(object sender, EventArgs e) {
            if ( copy_to_clipboard)
                Clipboard.SetText( util.color_to_str(SelectedColor) );
            DialogResult = DialogResult.OK;
        }

        private void cancel_Click(object sender, EventArgs e) {
            DialogResult = DialogResult.Cancel;
        }
    }
}
