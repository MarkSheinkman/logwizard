﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using BrightIdeasSoftware;

namespace lw_common.ui {
    internal class log_view_item_draw_ui {
        private solid_brush_list brush_ = new solid_brush_list();
        private gradient_brush_list gradient_ = new gradient_brush_list();

        private log_view parent_;

        private Font font_, b_font, bi_font, i_font;


        public log_view_item_draw_ui(log_view parent) {
            parent_ = parent;

            font_ = parent_.list.Font;
            b_font = new Font(font_.FontFamily, font_.Size, FontStyle.Bold);
            bi_font = new Font(font_.FontFamily, font_.Size, FontStyle.Bold | FontStyle.Italic);
            i_font = new Font(font_.FontFamily, font_.Size, FontStyle.Italic);
        }

        private void build_fonts() {
            b_font = new Font(font_.FontFamily, font_.Size, FontStyle.Bold);
            bi_font = new Font(font_.FontFamily, font_.Size, FontStyle.Bold | FontStyle.Italic);
            i_font = new Font(font_.FontFamily, font_.Size, FontStyle.Italic);            
        }

        public void set_font(Font f) {
            font_ = f;
            build_fonts();
        }
        public Font font(print_info print) {
            Font f = print.bold ? (print.italic ? bi_font : b_font) : (print.italic ? i_font : font_);
            return f;
        }

        public int text_width(Graphics g, string text) {
            // IMPORTANT: at this time, we assume we have a fixed font
            bool ends_in_space = text.EndsWith(" ");
            if (ends_in_space)
                // just append any character, so that the spaces are taken into account
                text += "_";

            int width = (int)g.MeasureString(text, font_).Width + 1;
            if (ends_in_space) {
                int avg_per_char = width / text.Length;
                width -= avg_per_char;
            }
            return width;
//            return char_size(g) * text.Length;
        }


        public int char_size(Graphics g) {
            // IMPORTANT: at this time, we assume we have a fixed font
            var ab = g.MeasureString("ab", font_).Width;
            var abc = g.MeasureString("abc", font_).Width;
            var abcd = g.MeasureString("abcd", font_).Width;
            Debug.Assert( (int)((abc - ab) * 100) == (int)((abcd - abc) * 100)) ;

            return (int)(abc - ab);
        }

        public Color print_bg_color(OLVListItem item, print_info print) {
            log_view.item i = item.RowObject as log_view.item;            
            Color bg = print.bg != util.transparent ? print.bg : util.darker_color( i.bg(parent_) );
            return bg;
        }

        public Brush print_bg_brush(OLVListItem item, print_info print) {
            return brush_.brush(print_bg_color(item, print));
        }

        public Color print_fg_color(OLVListItem item, print_info print) {
            log_view.item i = item.RowObject as log_view.item;            
            Color fg = i.fg(parent_);
            Color print_fg = print.fg != util.transparent ? print.fg : fg;
            return print_fg;
        }

        public Brush print_fg_brush(OLVListItem item, print_info print) {
            return brush_.brush(print_fg_color(item, print));
        }

        public Color bg_color(OLVListItem item, int col_idx) {
            log_view.item i = item.RowObject as log_view.item;
            int row_idx = item.Index;

            Color color;
            bool is_sel = parent_.multi_sel_idx.Contains(row_idx);

            Color bg = i.bg(parent_);
            Color dark_bg = i.sel_bg(parent_);

            if (col_idx == parent_.msgCol.Index) {
                if (is_sel) 
                    color = is_sel ? dark_bg : bg;
                else if (app.inst.use_bg_gradient) {
                    Rectangle r = item.GetSubItemBounds(col_idx);
                    if (r.Width > 0 && r.Height > 0)
                        // it's a gradient
                        color = util.transparent;
                    else
                        color = bg;
                } else
                    color = bg;
            } else 
                color = is_sel ? dark_bg : bg;

            return color;
        }

        public Brush bg_brush(OLVListItem item, int col_idx) {
            log_view.item i = item.RowObject as log_view.item;
            int row_idx = item.Index;

            if (col_idx == parent_.msgCol.Index) {
                bool is_sel = parent_.multi_sel_idx.Contains(row_idx);
                if (!is_sel && app.inst.use_bg_gradient) {
                    Rectangle r = item.GetSubItemBounds(col_idx);
                    if ( r.Width > 0 && r.Height > 0)
                        return gradient_.brush(r, app.inst.bg_from, app.inst.bg_to);
                }
            }

            return brush_.brush( bg_color(item, col_idx));
        }
    }
}