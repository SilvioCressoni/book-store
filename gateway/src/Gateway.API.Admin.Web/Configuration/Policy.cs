using System;

namespace Gateway.API.Admin.Web.Configuration
{
    public class Policy : IEquatable<Policy>
    {
        public double FailureThreshold { get; set; }
        public TimeSpan SamplingDuration { get; set; }
        public int MinimumThroughput { get; set; }
        public TimeSpan DurationOfBreak { get; set; }

        public bool Equals(Policy other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return FailureThreshold.Equals(other.FailureThreshold) 
                            && SamplingDuration.Equals(other.SamplingDuration) 
                            && MinimumThroughput == other.MinimumThroughput 
                            && DurationOfBreak.Equals(other.DurationOfBreak);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != this.GetType())
            {
                return false;
            }

            return Equals((Policy) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(FailureThreshold, SamplingDuration, MinimumThroughput, DurationOfBreak);
        }
    }
}
