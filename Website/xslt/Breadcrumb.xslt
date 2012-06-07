<?xml version="1.0" encoding="UTF-8"?>
<!DOCTYPE xsl:stylesheet [  <!ENTITY nbsp "&#x00A0;">]>
<xsl:stylesheet
  version="1.0"
  xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
  xmlns:msxml="urn:schemas-microsoft-com:xslt" 
  xmlns:umbraco.library="urn:umbraco.library" xmlns:Exslt.ExsltCommon="urn:Exslt.ExsltCommon" xmlns:Exslt.ExsltDatesAndTimes="urn:Exslt.ExsltDatesAndTimes" xmlns:Exslt.ExsltMath="urn:Exslt.ExsltMath" xmlns:Exslt.ExsltRegularExpressions="urn:Exslt.ExsltRegularExpressions" xmlns:Exslt.ExsltStrings="urn:Exslt.ExsltStrings" xmlns:Exslt.ExsltSets="urn:Exslt.ExsltSets" xmlns:PS.XSLTsearch="urn:PS.XSLTsearch" 
  exclude-result-prefixes="msxml umbraco.library Exslt.ExsltCommon Exslt.ExsltDatesAndTimes Exslt.ExsltMath Exslt.ExsltRegularExpressions Exslt.ExsltStrings Exslt.ExsltSets PS.XSLTsearch ">

  <xsl:output method="xml" omit-xml-declaration="yes"/>

  <xsl:param name="currentPage"/>

  <xsl:variable name="minLevel" select="0"/>

  <xsl:template match="/">

    <xsl:if test="$currentPage/@level &gt; $minLevel">
      <xsl:if test="count($currentPage/ancestor::* [@level &gt; $minLevel and string(umbracoNaviHide) != '1']) &gt; 0">
          <div id="kruimelpad" class="grid_12">
            <xsl:for-each select="$currentPage/ancestor::* [@level &gt; $minLevel and string(umbracoNaviHide) != '1' and name() != 'ForumDateFolder']">
              <xsl:if test="@level &gt; $minLevel + 1">
                &gt;
              </xsl:if>  
              <a class="c2" href="{umbraco.library:NiceUrl(@id)}"><xsl:value-of select="@nodeName"/></a>
            </xsl:for-each>
          </div>
        </xsl:if>
    </xsl:if>
    
  </xsl:template>
</xsl:stylesheet>