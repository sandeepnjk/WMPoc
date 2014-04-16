using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Dynamic;
using System.Reflection;

using CoreFramework.Models;
using CoreFramework.Utils;

namespace CoreFramework.Builders
{
    class PropertyBuilder : iBuilder
    {

        public T build<T>(ComplexTypeModel compType, IDictionary<string, object> jsonAsObject, T objectAtHand)
        {
            Dictionary<string, PropertyModel> allPropertiesInCompType = compType.getAllPropertiesInThisComplexType();

            foreach (KeyValuePair<string, PropertyModel> modelPair in allPropertiesInCompType)
            {
                PropertyModel propertyAtHand = modelPair.Value;
                objectAtHand = buildIndividualProperty(compType, propertyAtHand, jsonAsObject, objectAtHand);
            }

            return objectAtHand;
        }

        private T buildIndividualProperty<T>(ComplexTypeModel compType, PropertyModel propertyAtHand,
        IDictionary<string, object> jsonAsObject, T objectAtHand)
        {

            if (TypeUtil.isCollectionType(propertyAtHand.getPropertyType()))
            {
                objectAtHand = buildCollectionProperty(objectAtHand, jsonAsObject);
            }

            if (TypeUtil.isArrayType(propertyAtHand.getPropertyType()))
            {
                buildArrayProperty(compType, objectAtHand, jsonAsObject, propertyAtHand);
            }

            if (propertyAtHand.isComplexProperty())
            {
                objectAtHand = buildComplexProperty(compType, objectAtHand, jsonAsObject, propertyAtHand);
            }

            //At this stage its neither a collection/array or complex object

            objectAtHand = buildSimpleProperty(compType, objectAtHand, jsonAsObject, propertyAtHand);


            return objectAtHand;
        }

        private T buildCollectionProperty<T>(T objectAtHand, object jsonEquivalent)
        {


            return objectAtHand;
        }

        private T buildComplexProperty<T>(ComplexTypeModel compType, T objectAtHand,
                                                   IDictionary<string, object> jsonEquivalent, PropertyModel propertyAtHand)
        {
            objectAtHand = (T)ObjectProcessor.prepCompTypeForInvocation(compType.getDllFileThisTypeBelongsTo()
                             , propertyAtHand.getPropertyType() + "", jsonEquivalent);
            return objectAtHand;
        }

        private T buildArrayProperty<T>(ComplexTypeModel compType, T objectAtHand,
                                                   IDictionary<string, object> jsonEquivalent, PropertyModel propertyAtHand)
        {


            return objectAtHand;
        }

        private T buildSimpleProperty<T>(ComplexTypeModel compType, T objectAtHand,
                                           IDictionary<string, object> jsonObject, PropertyModel propertyAtHand)
        {
            object jsonEquivalent = jsonObject[propertyAtHand.getPropertyName()];

            //SimpleType -- So Json equivalent should be neither array, collection or complex object
            Type typeOfJsonEquivalentObject = jsonEquivalent.GetType();
            if (!TypeUtil.isArrayType(typeOfJsonEquivalentObject)
                    && !TypeUtil.isCollectionType(typeOfJsonEquivalentObject)
                        && !TypeUtil.isComplexType(typeOfJsonEquivalentObject))
            {
                PropertyInfo propertyInfo = objectAtHand.GetType().GetProperty(propertyAtHand.getPropertyName(),
                    BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance);

                object propertyValue = jsonEquivalent;
                try
                {
                    //Console.WriteLine("Printing Type of " + fieldAtHand.getFieldName() + " " + 
                    //    fieldAtHand.getFieldType() + " " + fieldValue.GetType());
                    propertyValue = Convert.ChangeType(propertyValue, propertyAtHand.getPropertyType());
                    propertyInfo.SetValue(objectAtHand, propertyValue);
                    Console.WriteLine("Setting Value " + " " + " " + propertyValue);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Issue assigning simple value " + ex.Message);
                }
            }
            else
            {
                throw new BuilderException("JSON not matching up to complex type " + propertyAtHand.getPropertyName() +
                       "Should not be array, collection or complex type instead found " +
                           typeOfJsonEquivalentObject);
            }

            return objectAtHand;
        }
    }
}
