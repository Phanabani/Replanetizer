﻿using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using LibReplanetizer.LevelObjects;
using OpenTK.Mathematics;

namespace Replanetizer.Utils
{
    public class Selection : ObservableCollection<LevelObject>
    {
        public Vector3 pivot
        {
            get
            {
                if (pivotDirty)
                {
                    _pivot = CalculatePivotPoint();
                    pivotDirty = false;
                }

                return _pivot;
            }
        }

        private bool pivotDirty;
        private Vector3 _pivot;

        public bool isOnlySplines => splinesCount > 1 && nonSplinesCount == 0;
        private int splinesCount;
        private int nonSplinesCount;

        private void SetDirty(bool dirty)
        {
            pivotDirty = dirty;
        }

        /// <summary>
        /// Force recalculate lazy properties (e.g. when object positions change)
        /// </summary>
        public void Update()
        {
            SetDirty(true);
        }

        public new void Clear()
        {
            splinesCount = 0;
            nonSplinesCount = 0;
            SetDirty(true);
            base.Clear();
        }

        public bool Set(LevelObject obj)
        {
            if (Count == 1 && ReferenceEquals(this[0], obj))
                return false;

            Clear();
            Add(obj);
            return true;
        }

        public new bool Add(LevelObject obj)
        {
            if (Contains(obj))
                return false;

            if (obj is Spline)
                splinesCount++;
            else
                nonSplinesCount++;

            SetDirty(true);
            base.Add(obj);
            return true;
        }

        public new bool Remove(LevelObject obj)
        {
            if (!base.Remove(obj))
                return false;

            if (obj is Spline)
                splinesCount--;
            else
                nonSplinesCount--;

            SetDirty(true);
            return true;
        }

        public bool Toggle(LevelObject obj)
        {
            return Add(obj) || Remove(obj);
        }

        public bool ToggleOne(LevelObject obj)
        {
            return Set(obj) || Remove(obj);
        }

        public bool TryGetOne([NotNullWhen(true)] out LevelObject? obj)
        {
            obj = null;
            if (Count != 1)
                return false;

            obj = this[0];
            return true;
        }

        private Vector3 CalculatePivotPoint()
        {
            var pivot = new Vector3();
            foreach (var obj in this)
            {
                pivot += obj.position;
            }
            pivot /= Count;
            return pivot;
        }
    }
}
