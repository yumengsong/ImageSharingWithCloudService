<?xml version="1.0" encoding="utf-8"?>
<serviceModel xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" name="ImageSharingWithCloudService.Azure1" generation="1" functional="0" release="0" Id="26d244a3-66ed-4ae3-9cce-c685239da118" dslVersion="1.2.0.0" xmlns="http://schemas.microsoft.com/dsltools/RDSM">
  <groups>
    <group name="ImageSharingWithCloudService.Azure1Group" generation="1" functional="0" release="0">
      <componentports>
        <inPort name="ImageSharingWebRole:Endpoint1" protocol="http">
          <inToChannel>
            <lBChannelMoniker name="/ImageSharingWithCloudService.Azure1/ImageSharingWithCloudService.Azure1Group/LB:ImageSharingWebRole:Endpoint1" />
          </inToChannel>
        </inPort>
      </componentports>
      <settings>
        <aCS name="ImageSharingWebRole:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/ImageSharingWithCloudService.Azure1/ImageSharingWithCloudService.Azure1Group/MapImageSharingWebRole:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </maps>
        </aCS>
        <aCS name="ImageSharingWebRoleInstances" defaultValue="[1,1,1]">
          <maps>
            <mapMoniker name="/ImageSharingWithCloudService.Azure1/ImageSharingWithCloudService.Azure1Group/MapImageSharingWebRoleInstances" />
          </maps>
        </aCS>
        <aCS name="ImageSharingWorkerRole:Microsoft.ServiceBus.ConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/ImageSharingWithCloudService.Azure1/ImageSharingWithCloudService.Azure1Group/MapImageSharingWorkerRole:Microsoft.ServiceBus.ConnectionString" />
          </maps>
        </aCS>
        <aCS name="ImageSharingWorkerRole:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/ImageSharingWithCloudService.Azure1/ImageSharingWithCloudService.Azure1Group/MapImageSharingWorkerRole:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </maps>
        </aCS>
        <aCS name="ImageSharingWorkerRoleInstances" defaultValue="[1,1,1]">
          <maps>
            <mapMoniker name="/ImageSharingWithCloudService.Azure1/ImageSharingWithCloudService.Azure1Group/MapImageSharingWorkerRoleInstances" />
          </maps>
        </aCS>
      </settings>
      <channels>
        <lBChannel name="LB:ImageSharingWebRole:Endpoint1">
          <toPorts>
            <inPortMoniker name="/ImageSharingWithCloudService.Azure1/ImageSharingWithCloudService.Azure1Group/ImageSharingWebRole/Endpoint1" />
          </toPorts>
        </lBChannel>
      </channels>
      <maps>
        <map name="MapImageSharingWebRole:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/ImageSharingWithCloudService.Azure1/ImageSharingWithCloudService.Azure1Group/ImageSharingWebRole/Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </setting>
        </map>
        <map name="MapImageSharingWebRoleInstances" kind="Identity">
          <setting>
            <sCSPolicyIDMoniker name="/ImageSharingWithCloudService.Azure1/ImageSharingWithCloudService.Azure1Group/ImageSharingWebRoleInstances" />
          </setting>
        </map>
        <map name="MapImageSharingWorkerRole:Microsoft.ServiceBus.ConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/ImageSharingWithCloudService.Azure1/ImageSharingWithCloudService.Azure1Group/ImageSharingWorkerRole/Microsoft.ServiceBus.ConnectionString" />
          </setting>
        </map>
        <map name="MapImageSharingWorkerRole:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/ImageSharingWithCloudService.Azure1/ImageSharingWithCloudService.Azure1Group/ImageSharingWorkerRole/Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </setting>
        </map>
        <map name="MapImageSharingWorkerRoleInstances" kind="Identity">
          <setting>
            <sCSPolicyIDMoniker name="/ImageSharingWithCloudService.Azure1/ImageSharingWithCloudService.Azure1Group/ImageSharingWorkerRoleInstances" />
          </setting>
        </map>
      </maps>
      <components>
        <groupHascomponents>
          <role name="ImageSharingWebRole" generation="1" functional="0" release="0" software="G:\Prithvi\ImageSharingWithCloudService\ImageSharingWithCloudService.Azure1\csx\Release\roles\ImageSharingWebRole" entryPoint="base\x64\WaHostBootstrapper.exe" parameters="base\x64\WaIISHost.exe " memIndex="-1" hostingEnvironment="frontendadmin" hostingEnvironmentVersion="2">
            <componentports>
              <inPort name="Endpoint1" protocol="http" portRanges="51331" />
            </componentports>
            <settings>
              <aCS name="Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="" />
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;ImageSharingWebRole&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;ImageSharingWebRole&quot;&gt;&lt;e name=&quot;Endpoint1&quot; /&gt;&lt;/r&gt;&lt;r name=&quot;ImageSharingWorkerRole&quot; /&gt;&lt;/m&gt;" />
            </settings>
            <resourcereferences>
              <resourceReference name="DiagnosticStore" defaultAmount="[4096,4096,4096]" defaultSticky="true" kind="Directory" />
              <resourceReference name="EventStore" defaultAmount="[1000,1000,1000]" defaultSticky="false" kind="LogStore" />
            </resourcereferences>
          </role>
          <sCSPolicy>
            <sCSPolicyIDMoniker name="/ImageSharingWithCloudService.Azure1/ImageSharingWithCloudService.Azure1Group/ImageSharingWebRoleInstances" />
            <sCSPolicyUpdateDomainMoniker name="/ImageSharingWithCloudService.Azure1/ImageSharingWithCloudService.Azure1Group/ImageSharingWebRoleUpgradeDomains" />
            <sCSPolicyFaultDomainMoniker name="/ImageSharingWithCloudService.Azure1/ImageSharingWithCloudService.Azure1Group/ImageSharingWebRoleFaultDomains" />
          </sCSPolicy>
        </groupHascomponents>
        <groupHascomponents>
          <role name="ImageSharingWorkerRole" generation="1" functional="0" release="0" software="G:\Prithvi\ImageSharingWithCloudService\ImageSharingWithCloudService.Azure1\csx\Release\roles\ImageSharingWorkerRole" entryPoint="base\x64\WaHostBootstrapper.exe" parameters="base\x64\WaWorkerHost.exe " memIndex="-1" hostingEnvironment="consoleroleadmin" hostingEnvironmentVersion="2">
            <settings>
              <aCS name="Microsoft.ServiceBus.ConnectionString" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="" />
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;ImageSharingWorkerRole&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;ImageSharingWebRole&quot;&gt;&lt;e name=&quot;Endpoint1&quot; /&gt;&lt;/r&gt;&lt;r name=&quot;ImageSharingWorkerRole&quot; /&gt;&lt;/m&gt;" />
            </settings>
            <resourcereferences>
              <resourceReference name="DiagnosticStore" defaultAmount="[4096,4096,4096]" defaultSticky="true" kind="Directory" />
              <resourceReference name="EventStore" defaultAmount="[1000,1000,1000]" defaultSticky="false" kind="LogStore" />
            </resourcereferences>
          </role>
          <sCSPolicy>
            <sCSPolicyIDMoniker name="/ImageSharingWithCloudService.Azure1/ImageSharingWithCloudService.Azure1Group/ImageSharingWorkerRoleInstances" />
            <sCSPolicyUpdateDomainMoniker name="/ImageSharingWithCloudService.Azure1/ImageSharingWithCloudService.Azure1Group/ImageSharingWorkerRoleUpgradeDomains" />
            <sCSPolicyFaultDomainMoniker name="/ImageSharingWithCloudService.Azure1/ImageSharingWithCloudService.Azure1Group/ImageSharingWorkerRoleFaultDomains" />
          </sCSPolicy>
        </groupHascomponents>
      </components>
      <sCSPolicy>
        <sCSPolicyUpdateDomain name="ImageSharingWebRoleUpgradeDomains" defaultPolicy="[5,5,5]" />
        <sCSPolicyUpdateDomain name="ImageSharingWorkerRoleUpgradeDomains" defaultPolicy="[5,5,5]" />
        <sCSPolicyFaultDomain name="ImageSharingWebRoleFaultDomains" defaultPolicy="[2,2,2]" />
        <sCSPolicyFaultDomain name="ImageSharingWorkerRoleFaultDomains" defaultPolicy="[2,2,2]" />
        <sCSPolicyID name="ImageSharingWebRoleInstances" defaultPolicy="[1,1,1]" />
        <sCSPolicyID name="ImageSharingWorkerRoleInstances" defaultPolicy="[1,1,1]" />
      </sCSPolicy>
    </group>
  </groups>
  <implements>
    <implementation Id="e03a0305-ba9e-487c-aa15-da6349d81e79" ref="Microsoft.RedDog.Contract\ServiceContract\ImageSharingWithCloudService.Azure1Contract@ServiceDefinition">
      <interfacereferences>
        <interfaceReference Id="0cc0062e-8769-4bc2-84e8-019d570c422e" ref="Microsoft.RedDog.Contract\Interface\ImageSharingWebRole:Endpoint1@ServiceDefinition">
          <inPort>
            <inPortMoniker name="/ImageSharingWithCloudService.Azure1/ImageSharingWithCloudService.Azure1Group/ImageSharingWebRole:Endpoint1" />
          </inPort>
        </interfaceReference>
      </interfacereferences>
    </implementation>
  </implements>
</serviceModel>