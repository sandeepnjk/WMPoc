<?xml version='1.0' ?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:template match="/">
	<html>
	<head>
	<title> Documentation for DLL <xsl:value-of select="dll/dllFileName"/> </title>
	</head>
	<body>
		<table width="800" border="1" >
			<tr bgcolor = "#cccccc" >
			  <td>
			    DLL File Name
			  </td>
			  <td>
			    <xsl:value-of select="dll/dllFileName"/>
			  </td>
			</tr>
			<tr bgcolor = "#cccccc" >
			  <td>
			    DLL Fully Qualified Path
			  </td>
			  <td>
				<xsl:value-of select="dll/fullyQualifiedPath"/>
			  </td>
			</tr>
			<tr bgcolor = "#cccccc" >
			  <td>
			    Is DLL a Managed (MSIL and non-native) Assembly ?
			  </td>
			  <td>
			<xsl:value-of select="dll/isManaged"/>
			  </td>
			</tr>
		</table>
		<br/>
        <br/>

		<table width="800" border="1">
		  <tr bgcolor = "#cccccc"><td width = "20%"> </td><td><center> <b> User selected classes in this DLL </b> </center></td></tr>
		    <xsl:for-each select="dll/userSelectedClassesInDll">
			  <xsl:for-each select="class">
				  <tr bgcolor = "#FAFAFA">
				  <td width = "20%">
					<b>Class Name</b>
				  </td>
				    <td width = "80%">
					<xsl:value-of select="className"/>
				  </td>
				  </tr>
				  <tr bgcolor = "#FAFAFA">
				  <td width = "20%">
					Controller Alias
				  </td>
				  <td width = "80%">
					<xsl:value-of select="controllerAlias"/>
				  </td>
				</tr>
				  <tr bgcolor = "#FAFAFA">
				    <td width = "20%">
					Endpoint
				  </td>
				    <td width = "80%">
					http://localhost:60064/<xsl:value-of select="controllerAlias"/>/{Action}
				  </td>
				  </tr>
			        <xsl:for-each select="methods/method">			
			          <tr bgcolor = "D0FA58">
			            <td>
			              Method
			            </td>
         	            <td>
			              <table width = "680" border="0">
				            <tr bgcolor = "D0FA58">
				              <td width = "30%">
					            Method Name
				              </td>
				              <td width = "70%">
					            <xsl:value-of select="methodName"/>
				              </td>
				            </tr>
				            <tr bgcolor = "D0FA58">
				              <td>
					          Action Alias
				              </td>
				              <td>
					            <xsl:value-of select="actionAlias"/>
				              </td>
				            </tr>
							<tr bgcolor = "D0FA58">
				              <td width = "30%">
					            Method Return Type
				              </td>
				              <td width = "70%">
					            <xsl:value-of select="methodReturnType"/>
				              </td>
							</tr>

							<xsl:for-each select="parameters/parameter">
							  <tr bgcolor = "#FBF5EF">
							    <td>
							      Param
							    </td>
								<td>
								  <table width = "500" border="1">
								    <tr bgcolor = "#FBF5EF">
									  <td width="30%">
									     Parameter Name
									  </td>
									  <td width="70%">
									    <xsl:value-of select="parameterName"/>
									  </td>
									</tr>
								    <tr bgcolor = "#FBF5EF">
									  <td width="30%">
									     Parameter Type
									  </td>
									  <td width="70%">
									    <xsl:value-of select="parameterType"/>
									  </td>
									</tr>
								    <tr bgcolor = "#FBF5EF">
									  <td width="30%">
									     Position of Parameter
									  </td>
									  <td width="70%">
									    <xsl:value-of select="positionOfParameter"/>
									  </td>
									</tr>
								  </table>
								</td>
							  </tr>
							  </xsl:for-each>
							</table>
					      </td>
			            </tr>	 
				     </xsl:for-each>		           
	          </xsl:for-each>			  
	        </xsl:for-each>	
	</table>
	</body>
	</html>
	</xsl:template>



</xsl:stylesheet><!-- Stylus Studio meta-information - (c) 2004-2009. Progress Software Corporation. All rights reserved.

<metaInformation>
	<scenarios>
		<scenario default="yes" name="Scenario1" userelativepaths="yes" externalpreview="no" url="ClassLibrary2.dll.xml" htmlbaseurl="" outputurl="output" processortype="saxon8" useresolver="yes" profilemode="0" profiledepth="" profilelength=""
		          urlprofilexml="" commandline="" additionalpath="" additionalclasspath="" postprocessortype="none" postprocesscommandline="" postprocessadditionalpath="" postprocessgeneratedext="" validateoutput="no" validator="internal"
		          customvalidator="">
			<advancedProp name="sInitialMode" value=""/>
			<advancedProp name="schemaCache" value="||"/>
			<advancedProp name="bSchemaAware" value="true"/>
			<advancedProp name="bXsltOneIsOkay" value="true"/>
			<advancedProp name="bXml11" value="false"/>
			<advancedProp name="bGenerateByteCode" value="true"/>
			<advancedProp name="iValidation" value="0"/>
			<advancedProp name="bExtensions" value="true"/>
			<advancedProp name="iWhitespace" value="0"/>
			<advancedProp name="sInitialTemplate" value=""/>
			<advancedProp name="bTinyTree" value="true"/>
			<advancedProp name="bUseDTD" value="false"/>
			<advancedProp name="bWarnings" value="true"/>
			<advancedProp name="xsltVersion" value="2.0"/>
			<advancedProp name="iErrorHandling" value="fatal"/>
		</scenario>
	</scenarios>
	<MapperMetaTag>
		<MapperInfo srcSchemaPathIsRelative="yes" srcSchemaInterpretAsXML="no" destSchemaPath="" destSchemaRoot="" destSchemaPathIsRelative="yes" destSchemaInterpretAsXML="no">
			<SourceSchema srcSchemaPath="ClassLibrary2.dll.xml" srcSchemaRoot="dll" AssociatedInstance="" loaderFunction="document" loaderFunctionUsesURI="no"/>
		</MapperInfo>
		<MapperBlockPosition>
			<template match="/">
				<block path="html/body/table[1]/xsl:for-each" x="400" y="130"/>
				<block path="html/body/table[1]/xsl:for-each/xsl:for-each" x="350" y="160"/>
				<block path="html/body/table[1]/xsl:for-each/xsl:for-each/xsl:for-each" x="310" y="160"/>
				<block path="html/body/table[1]/xsl:for-each/xsl:for-each/xsl:for-each/tr/td[1]/table/tr/td[1]/xsl:value-of" x="270" y="160"/>
				<block path="html/body/table[1]/xsl:for-each/xsl:for-each/xsl:for-each/tr/td[1]/table/tr[1]/td[1]/xsl:value-of" x="230" y="160"/>
			</template>
		</MapperBlockPosition>
		<TemplateContext></TemplateContext>
		<MapperFilter side="source"></MapperFilter>
	</MapperMetaTag>
</metaInformation>
-->