using System;
using System.Collections.Generic;
using FireworksNet.Extensions;

namespace FireworksNet.Model
{
    /// <summary>
    /// Represents a solution - a point in problem space defined by coordinates
    /// and corresponding quality (value of target function).
    /// </summary>
    public class Solution : IEquatable<Solution>
    {
        /// <summary>
        /// Gets solution coordinates in problem space.
        /// </summary>
        public IDictionary<Dimension, double> Coordinates { get; private set; } // TODO: Think of replacing Dictionary with some derived class, like CoordinateDictionary

        /// <summary>
        /// Gets or sets solution quality (value of target function).
        /// </summary>
        public double Quality { get; set; }

        /// <summary>
        /// Initializes a new instance of <see cref="Solution"/> with
        /// defined coordinates and quality.
        /// </summary>
        /// <param name="coordinates">Solution coordinates in problem space.</param>
        /// <param name="quality">Solution quality (value of target function).</param>
        public Solution(IDictionary<Dimension, double> coordinates, double quality)
        {
            this.Coordinates = coordinates == null ? null : new Dictionary<Dimension, double>(coordinates);
            this.Quality = quality;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="Solution"/> with
        /// defined coordinates.
        /// </summary>
        /// <param name="coordinates">Solution coordinates in problem space.</param>
        /// <remarks><see cref="Quality"/> is set to <see cref="double.NaN"/>.</remarks>
        public Solution(IDictionary<Dimension, double> coordinates)
            : this(coordinates, double.NaN)
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="Solution"/> with
        /// defined quality.
        /// </summary>
        /// <param name="quality">Solution quality (value of target function).</param>
        /// <remarks><see cref="Coordinates"/> is set to <c>null</c>.</remarks>
        public Solution(double quality)
            : this(null, quality)
        {
        }

        #region Object overrides

        /// <summary>
        /// Determines whether the specified <see cref="Solution"/> is equal to the current one.
        /// </summary>
        /// <param name="obj">The <see cref="Solution"/> object to compare with the current one.</param>
        /// <returns><c>true</c> if the specified <see cref="Solution"/> is equal to the current one;
        /// otherwise <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            return this.Equals(obj as Solution);
        }

        /// <summary>
        /// Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>A hash code for the current <see cref="Solution"/>.</returns>
        public override int GetHashCode()
        {
            // http://stackoverflow.com/questions/263400/what-is-the-best-algorithm-for-an-overridden-system-object-gethashcode
            unchecked
            {
                int hash = 17;
                hash = hash * 29 + this.Coordinates.GetHashCode();
                hash = hash * 29 + this.Quality.GetHashCode();

                return hash;
            }
        }

        #endregion

        #region Comparison operators

        /// <summary>
        /// Determines whether values of two instances of <see cref="Solution"/> are equal.
        /// </summary>
        /// <param name="left">First instance of <see cref="Solution"/>.</param>
        /// <param name="right">Second instance of <see cref="Solution"/>.</param>
        /// <returns><c>true</c> if <paramref name="left"/> value is equal to the <paramref name="right"/> 
        /// value; otherwise <c>false</c>.</returns>
        public static bool operator ==(Solution left, Solution right)
        {
            if (object.ReferenceEquals((object)left, (object)null))
            {
                return object.ReferenceEquals((object)right, (object)null);
            }

            return left.Equals(right);
        }

        /// <summary>
        /// Determines whether values of two instances of <see cref="Solution"/> are not equal.
        /// </summary>
        /// <param name="left">First instance of <see cref="Solution"/>.</param>
        /// <param name="right">Second instance of <see cref="Solution"/>.</param>
        /// <returns><c>true</c> if <paramref name="left"/> value is not equal to the <paramref name="right"/> 
        /// value; otherwise <c>false</c>.</returns>
        public static bool operator !=(Solution left, Solution right)
        {
            if (object.ReferenceEquals((object)left, (object)null))
            {
                return !object.ReferenceEquals((object)right, (object)null);
            }

            return !left.Equals(right);
        }

        #endregion

        #region IEquatable<Solution>

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns><c>true</c> if the current object is equal to the other parameter; otherwise <c>false</c>.</returns>
        public bool Equals(Solution other)
        {
            if (object.ReferenceEquals((object)other, (object)null))
            {
                return false;
            }

            if (object.ReferenceEquals((object)other, (object)this))
            {
                return true;
            }

            return (this.Coordinates.Equals(other.Coordinates)) && // TODO: Need to compare dictionaries contents, not references.
                   (this.Quality.IsEqual(other.Quality));
        }

        #endregion
    }
}