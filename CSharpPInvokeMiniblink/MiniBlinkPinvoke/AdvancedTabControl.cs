using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows;
using System.Windows.Forms;

namespace MiniBlinkPinvoke
{
    public delegate void TabEventHandler(object sender, TabEventArgs e);
    public delegate void TabCancelEventHandler(object sender, TabCancelEventArgs e);

    public class AdvancedTabControl : TabControl
    {
        public AdvancedTabControl()
        {
            this.AllowClose = true;
            this.AllowSwap = true;
        }

        #region Tab Swapping and Tab Closing

        private TabPage predraggedTab, contextmenuTab;
        private Point dragStartPoint;
        private readonly ContextMenu tabCtxm = new ContextMenu();
        private bool allowClose, allowSwap;



        [DefaultValue(true)]
        [Category("Behavior")]
        [Description("Indicates wether tabs in the control can be manually closed by the user.")]
        public virtual bool AllowClose
        {
            get
            {
                return allowClose;
            }
            set
            {
                allowClose = value;
                rebuildContextMenu();
            }
        }

        [DefaultValue(true)]
        [Category("Behavior")]
        [Description("Indicates wether tabs in the control can be manually rearranged by the user. \nRequires AllowDrop to be enabled.")]
        [RefreshProperties(RefreshProperties.All)]
        public virtual bool AllowSwap
        {
            get
            {
                return allowSwap;
            }
            set
            {
                allowSwap = value;

                if (!this.AllowDrop && value)
                    this.AllowDrop = true;
            }
        }

        [DefaultValue(true)]
        [RefreshProperties(RefreshProperties.All)]
        public override bool AllowDrop
        {
            get
            {
                return base.AllowDrop;
            }
            set
            {
                base.AllowDrop = value;

                if (!value && this.AllowSwap)
                    this.AllowSwap = false;
            }
        }



        protected override void OnMouseDown(MouseEventArgs e)
        {
            predraggedTab = getPointedTab(e.Location);
            dragStartPoint = e.Location;

            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            predraggedTab = null;

            TabPage pointedTab = getPointedTab(e.Location);
            if (this.AllowClose && pointedTab != null)
            {
                if (e.Button == MouseButtons.Right)
                {
                    contextmenuTab = pointedTab;
                    tabCtxm.Show(this, e.Location);
                }

                if (e.Button == MouseButtons.Middle)
                {
                    contextmenuTab = pointedTab;
                    closeTab(this, e);
                }
            }

            base.OnMouseUp(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (this.AllowSwap && predraggedTab != null && e.Button == MouseButtons.Left && isDragGesture(dragStartPoint, e.Location))
                this.DoDragDrop(predraggedTab, DragDropEffects.Move);

            base.OnMouseMove(e);
        }

        protected override void OnDragOver(DragEventArgs drgevent)
        {
            TabPage draggedTab = (TabPage)drgevent.Data.GetData(typeof(TabPage));
            TabPage pointedTab = getPointedTab(this.PointToClient(new Point(drgevent.X, drgevent.Y)));

            if (draggedTab == predraggedTab && pointedTab != null)
            {
                drgevent.Effect = DragDropEffects.Move;

                if (pointedTab != draggedTab)
                    swapTabPages(draggedTab, pointedTab);
            }

            base.OnDragOver(drgevent);
        }



        private bool isDragGesture(Point start, Point current)
        {
            return Math.Abs(start.X - current.X) > SystemParameters.MinimumHorizontalDragDistance || Math.Abs(start.Y - current.Y) > SystemParameters.MinimumVerticalDragDistance;
        }

        private void rebuildContextMenu()
        {
            tabCtxm.MenuItems.Clear();

            if (this.AllowClose)
                tabCtxm.MenuItems.Add("Close", closeTab);
        }

        private void closeTab(object sender, EventArgs e)
        {
            this.OnTabClosing(new TabCancelEventArgs(contextmenuTab));
            contextmenuTab = null;
        }

        private void swapTabPages(TabPage src, TabPage dst)
        {
            int srci = this.TabPages.IndexOf(src);
            int dsti = this.TabPages.IndexOf(dst);

            this.TabPages[dsti] = src;
            this.TabPages[srci] = dst;

            if (this.SelectedIndex == srci)
                this.SelectedIndex = dsti;
            else if (this.SelectedIndex == dsti)
                this.SelectedIndex = srci;

            this.Refresh();
        }

        #endregion

        #region Custom Events

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public event TabCancelEventHandler TabClosing;

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        public event TabEventHandler TabClosed;

        // TODO: Add tabs swapped event.

        // TODO: Add tab mouseup, mousedown, click etc... events.

        protected virtual void OnTabClosing(TabCancelEventArgs e)
        {
            if (this.TabClosing != null)
                this.TabClosing(this, e);

            if (!e.Cancel)
            {
                this.TabPages.Remove(e.Tab);
                this.OnTabClosed(new TabEventArgs(e.Tab));
            }
        }

        protected virtual void OnTabClosed(TabEventArgs e)
        {
            if (this.TabClosed != null)
                this.TabClosed(this, e);
        }

        #endregion

        private TabPage getPointedTab(Point position)
        {
            for (int i = 0; i < this.TabPages.Count; i++)
                if (this.GetTabRect(i).Contains(position))
                    return this.TabPages[i];

            return null;
        }
    }
}