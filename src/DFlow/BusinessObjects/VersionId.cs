// Copyright (C) 2022  Road to Agility
//
// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
// file, You can obtain one at https://mozilla.org/MPL/2.0/.

namespace DFlow.BusinessObjects
{
    public sealed class VersionId : ValueOf<int, VersionId>
    {
        public static readonly int VersionEmpty = 0;
        public static readonly int VersionInitial = 1;
        public static readonly int VersionIncrement = 1;

        public bool IsNew => Value == VersionInitial;

        public static VersionId Empty()
        {
            return From(VersionEmpty);
        }

        public static VersionId New()
        {
            return From(VersionInitial);
        }

        public static VersionId Next(VersionId current)
        {
            return From(current.Value + VersionIncrement);
        }

        public static bool operator >=(VersionId a, VersionId b)
            => a.Value >= b.Value;

        public static bool operator <=(VersionId a, VersionId b)
            => a.Value <= b.Value;

        public static bool operator >(VersionId a, VersionId b)
            => a.Value > b.Value;

        public static bool operator <(VersionId a, VersionId b)
            => a.Value < b.Value;
    }
}