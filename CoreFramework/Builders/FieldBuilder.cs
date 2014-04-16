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
    class FieldBuilder : iBuilder
    {
        public T build<T>(ComplexTypeModel compType, IDictionary<string, object> jsonAsObject, T objectAtHand)
        {
            Dictionary<string, FieldModel> allFieldsInCompType = compType.getAllFieldsInThisComplexType();

            foreach (KeyValuePair<string, FieldModel> modelPair in allFieldsInCompType)
            {
                FieldModel fieldAtHand = modelPair.Value;
                objectAtHand = buildIndividualField(compType, fieldAtHand, jsonAsObject, objectAtHand);
            }

            return objectAtHand;
        }

        private T buildIndividualField<T>(ComplexTypeModel compType, FieldModel fieldAtHand,
                IDictionary<string, object> jsonAsObject, T objectAtHand)
        {
            
            if (TypeUtil.isCollectionType(fieldAtHand.getFieldType())) {
                objectAtHand = buildCollectionField(objectAtHand, jsonAsObject);
            }

            if (TypeUtil.isArrayType(fieldAtHand.getFieldType()))
            {
                buildArrayField(compType, objectAtHand, jsonAsObject, fieldAtHand);
            }

            if (fieldAtHand.isComplexField())
            {
                object tempObject = jsonAsObject[fieldAtHand.getFieldName()];

                if (!tempObject.GetType().ToString().Equals(new ExpandoObject().GetType().ToString()))
                {
                    throw new BuilderException("JSON not matching up to complex type " + fieldAtHand.getFieldName() +
                       "Should be ExpandoObject but found " +
                           tempObject.GetType());
                }

                ExpandoObject tempExpandoObject = (ExpandoObject)tempObject;
                objectAtHand = buildComplexField(compType, objectAtHand, tempExpandoObject, fieldAtHand);
            }

            //At this stage its neither a collection/array or complex object

            objectAtHand = buildSimpleField(compType, objectAtHand, jsonAsObject, fieldAtHand);


            return objectAtHand;
        }

        private T buildCollectionField<T>(T objectAtHand, object jsonEquivalent)
        {


            return objectAtHand;
        }

        private T buildComplexField<T>(ComplexTypeModel compType, T objectAtHand,
                                                   ExpandoObject jsonEquivalent, FieldModel fieldAtHand)
        {
            Console.WriteLine(jsonEquivalent);
            //object jsonEquivalentObject = jsonEquivalent[fieldAtHand.getFieldName()];

            object valueObj = (T)ObjectProcessor.prepCompTypeForInvocation(compType.getDllFileThisTypeBelongsTo()
                             , fieldAtHand.getFieldType() + "", jsonEquivalent);

           FieldInfo fieldInfo = objectAtHand.GetType().GetField(fieldAtHand.getFieldName(),
                      BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance);
                Console.WriteLine("Setting a complex object of type " + valueObj.GetType()
                    + " to a complex field " + fieldAtHand.getFieldName() + " of type " + fieldInfo.FieldType);
           fieldInfo.SetValue(objectAtHand, valueObj);

            
            return objectAtHand;
        }

        private T buildArrayField<T>(ComplexTypeModel compType, T objectAtHand,
                                                   IDictionary<string, object> jsonEquivalent, FieldModel fieldAtHand)
        {


            return objectAtHand;
        }

        private T buildSimpleField<T> (ComplexTypeModel compType, T objectAtHand,
                                                   IDictionary<string, object> jsonObject, FieldModel fieldAtHand)
        {
            object jsonEquivalent = jsonObject[fieldAtHand.getFieldName()];

            //SimpleType -- So Json equivalent should be neither array, collection or complex object
            Type typeOfJsonEquivalentObject = jsonEquivalent.GetType();
            if (!TypeUtil.isArrayType(typeOfJsonEquivalentObject) 
                    && !TypeUtil.isCollectionType(typeOfJsonEquivalentObject)
                        && !TypeUtil.isComplexType(typeOfJsonEquivalentObject))
            {
                FieldInfo fieldInfo = objectAtHand.GetType().GetField(fieldAtHand.getFieldName(), 
                    BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance);

                object fieldValue = jsonEquivalent;
                try
                {
                    //Console.WriteLine("Printing Type of " + fieldAtHand.getFieldName() + " " + 
                    //    fieldAtHand.getFieldType() + " " + fieldValue.GetType());
                    fieldValue = Convert.ChangeType(fieldValue, fieldAtHand.getFieldType());
                    fieldInfo.SetValue(objectAtHand, fieldValue);
                    Console.WriteLine("Setting Value " + " " + " " + fieldValue);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Issue assigning simple value " + ex.Message);
                }                
            }
            else
            {
                throw new BuilderException("JSON not matching up to complex type " + fieldAtHand.getFieldName() + 
                       "Should not be array, collection or complex type instead found " +
                           typeOfJsonEquivalentObject.GetType());
            }
            
            return objectAtHand;
        }
    }
}
