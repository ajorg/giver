//***********************************************************************
// *  Preferences.cs
// *
// *  Copyright (C) 2007 Novell, Inc.
// *
// *  This program is free software; you can redistribute it and/or
// *  modify it under the terms of the GNU General Public
// *  License as published by the Free Software Foundation; either
// *  version 2 of the License, or (at your option) any later version.
// *
// *  This program is distributed in the hope that it will be useful,
// *  but WITHOUT ANY WARRANTY; without even the implied warranty of
// *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// *  General Public License for more details.
// *
// *  You should have received a copy of the GNU General Public
// *  License along with this program; if not, write to the Free
// *  Software Foundation, Inc., 675 Mass Ave, Cambridge, MA 02139, USA.
// *
// **********************************************************************

using System;
using System.IO;
using Gtk;
using Glade;
using Mono.Unix;

namespace Giver
{
	public class PreferencesDialog
	{
		private string window_name;
		private Glade.XML glade;
		private Window window;

		private Tooltips tips = new Tooltips();
		private FileChooserButton photo_location_chooser;

		public PreferencesDialog()
		{
			Glade.XML glade = new Glade.XML(Path.Combine(Defines.GladeDir, "giver-prefs.glade"), "GiverPrefsDialog", "giver");
			window_name = "giver-prefs";        
			this.glade = glade; 
			this.glade.Autoconnect(this);
			BuildWindow();
			LoadPreferences();
			ConnectEvents();
		}

		public virtual void Destroy()
		{
			Window.Destroy();
		}

		protected Glade.XML Glade
		{
			get { return glade; }
		}

		public string Name
		{
			get { return window_name; }
		}

		public Window Window
		{
			get {
			if(window == null) {
				window = (Window)glade.GetWidget(window_name);
			}

			return window;
			}
		}
		
		public ResponseType Run()
		{
		  return (ResponseType)Dialog.Run();
		}

		public Dialog Dialog
		{
			get { return (Dialog)Window; }
		}

		private void BuildWindow()
		{
			photo_location_chooser = new FileChooserButton("Select photo location",
			    FileChooserAction.Open);
			(Glade["photo_location_container"] as Container).Add(photo_location_chooser);
			(Glade["photo_location_label"] as Label).MnemonicWidget = photo_location_chooser;
			photo_location_chooser.Show();

			tips.SetTip(Glade["photo_location_label"], "Location of your photo", "photo_location");
		}

		private void LoadPreferences()
		{
			string location = Path.Combine(Environment.GetFolderPath(
			Environment.SpecialFolder.ApplicationData), "giver/preferences");

			
			string photoLocation = "/tmp"; // open key/value file location, parse key PhotoLocation 
			photo_location_chooser.SetFilename(photoLocation);

			//OnPhotoFileChanged(null, null);
		}

		private void ConnectEvents()
		{
			photo_location_chooser.SelectionChanged += delegate {
				Application.Preferences.PhotoLocation = photo_location_chooser.Filename;
			};

			//photo.Changed += OnPhotoFileChanged;
		}

		private void OnPhotoFileChanged(object o, EventArgs args)
		{
			(Glade["example_path"] as Label).Markup = String.Format("<small><i>{0}</i></small>",
			GLib.Markup.EscapeText("giver"));
		}
	}
}