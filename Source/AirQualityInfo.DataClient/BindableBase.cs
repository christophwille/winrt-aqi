using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AirQualityInfo.DataClient
{
    public class BindableBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName]string name = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        public void OnPropertyChanged(Expression<Func<object>> expression)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (handler != null)
            {
                if (expression.NodeType != ExpressionType.Lambda)
                {
                    throw new ArgumentException("Value must be a lamba expression");
                }

                var body = expression.Body as MemberExpression;

                if (body == null)
                {
                    throw new ArgumentException("Must be a member expression");
                }

                string propertyName = body.Member.Name;
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        // Copied from the old Windows 8 BindableBase common code
        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] String propertyName = null)
        {
            if (object.Equals(storage, value)) return false;

            storage = value;
            this.OnPropertyChanged(propertyName);
            return true;
        }
    }
}
