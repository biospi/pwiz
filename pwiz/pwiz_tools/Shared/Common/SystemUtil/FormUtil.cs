﻿/*
 * Original author: Nicholas Shulman <nicksh .at. u.washington.edu>,
 *                  MacCoss Lab, Department of Genome Sciences, UW
 *
 * Copyright 2015 University of Washington - Seattle, WA
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
using System.Windows.Forms;

namespace pwiz.Common.SystemUtil
{
    public static class FormUtil
    {
        /// <summary>
        /// Finds the top level form which is suitable to pass to <see cref="Form.ShowDialog(IWin32Window)"/>.
        /// This function looks for a form for which ShowInTaskBar is true.  When dialogs are shown that are owned
        /// by a popup form which is not ShowInTaskBar, it often prevents the user from Alt-Tabbing back to 
        /// the application.
        /// </summary>
        public static Control FindTopLevelOwner(Control control)
        {
            if (null == control)
            {
                return null;
            }
            var topLevelForm = control.TopLevelControl as Form;
            if (null == topLevelForm)
            {
                return control;
            }
            if (IsSuitableDialogOwner(topLevelForm))
            {
                return topLevelForm;
            }
            for (var formOwner = topLevelForm.Owner; null != formOwner; formOwner = formOwner.Owner)
            {
                if (IsSuitableDialogOwner(formOwner))
                {
                    return formOwner;
                }
            }
            return topLevelForm;
        }

        public static bool IsSuitableDialogOwner(Form form)
        {
            return form.ShowInTaskbar || form.Modal;
        }
    }
}