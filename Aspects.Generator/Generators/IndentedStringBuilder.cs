namespace RhoMicro.CodeAnalysis.Library.Text;
using System;

using RhoMicro.ApplicationFramework.Aspects;

partial class IndentedStringBuilder
{
    public IndentedStringBuilder AppendImplementationSignature(ServiceModel serviceModel) =>
        Append(Constants.ValueTaskTypeName).Append('<').Append(serviceModel.ResultTypeFullName).Append("> ")
        .Append(serviceModel.RequestTypeName).Append('(')
        .Append(b =>
        {
            foreach(var (type, name, _) in serviceModel.Parameters)
            {
                b.Append(type).Append(' ').Append(name).AppendCore(", ");
            }
        })
        .Append(Constants.CtTypeName).Append(" cancellationToken)");
    public IndentedStringBuilder Append(RequestTypeKind requestTypeKind) =>
        Append(requestTypeKind switch
        {
            RequestTypeKind.RecordStruct => "readonly partial record struct",
            RequestTypeKind.RecordClass => "partial record class",
            RequestTypeKind.SealedRecordClass => "sealed partial record class",
            RequestTypeKind.AbstractRecordClass => "abstract partial record class",
            _ => throw new InvalidOperationException($"Unable to append request type kind: '{requestTypeKind}'")
        });
    public IndentedStringBuilder Append(ServiceVisibility visibility) =>
        Append(visibility switch
        {
            ServiceVisibility.Public => "public",
            _ => "internal"
        });
    public IndentedStringBuilder AppendServiceType(ServiceModel serviceModel, SettingsModel settingsModel) =>
        Append(settingsModel.ServiceInterfaceTypeName).Append('<')
        .Append(serviceModel.RequestTypeFullName).Append(", ")
        .Append(serviceModel.ResultTypeFullName).Append('>');
    public IndentedStringBuilder AppendAdapterInvocation(ServiceModel serviceModel)
    {
        Append(serviceModel.RequestTypeName).AppendCore('(');

        var i = 0;
        foreach(var (_, paramName, propertyName) in serviceModel.Parameters)
        {
            Append(paramName).Append(": request.").AppendCore(propertyName);
            i++;
            if(i != serviceModel.Parameters.Count)
            {
                AppendCore(", ");
            }
        }

        if(serviceModel.CancellationTokenParameter.HasValue)
        {
            Append(", ").Append(serviceModel.CancellationTokenParameter.Value.Name).Append(": ")
                .AppendCore(serviceModel.CancellationTokenParameter.Value.Name);
        }

        AppendCore(')');

        return this;
    }
}
