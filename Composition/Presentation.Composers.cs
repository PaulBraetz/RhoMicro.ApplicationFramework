namespace RhoMicro.ApplicationFramework.Composition.Presentation;

using Microsoft.Extensions.Configuration;

using RhoMicro.ApplicationFramework.Common;
using RhoMicro.ApplicationFramework.Common.Abstractions;
using RhoMicro.ApplicationFramework.Presentation.Models;
using RhoMicro.ApplicationFramework.Presentation.Models.Abstractions;

using SimpleInjector;
using SimpleInjector.Diagnostics;

/// <summary>
/// Contains object graph definitions for presentation models.
/// </summary>
public static class Composers
{
    /// <summary>
    /// Gets a composer able to compose hosting information graphs.
    /// </summary>
    public static IComposer HostingInformation { get; } = Composer.Create(c =>
    {
        c.Register<IHostingInformation>(() =>
        {
            var config = c.GetInstance<IConfiguration>()
                .GetRequiredSection("HostingInformation");
            var scheme = config.GetRequiredSection("Scheme").Value ??
                throw new Exception("No configuration value provided for 'HostingInformation:Scheme'.");
            var host = config.GetRequiredSection("Host").Value ??
                throw new Exception("No configuration value provided for 'HostingInformation:Host'.");
            var port = config.GetRequiredInt32Value("Port");

            var result = new HostingInformation(host, scheme, port);

            return result;
        });
    });
    /// <summary>
    /// Gets a composer able to compose presentation model graphs.
    /// </summary>
    public static IComposer Models { get; } = Composer.Create(c =>
    {
        c.Register(typeof(IDisplayModel<>), typeof(DisplayModel<>));

        c.Register(typeof(IOptionsProvider<>), typeof(OptionsProviderAdapter<>), Lifestyle.Singleton);

        c.Register<IInputModel<String, String>, InputModel<String, String>>();
        c.Register<IInputModel<DateTime, String>, InputModel<DateTime, String>>();
        c.Register(typeof(IOptionModelFactory<>), typeof(OptionModelFactory<>), Lifestyle.Singleton);

        c.Register(typeof(IDefaultValueProviderFactory<>), typeof(DefaultValueProviderFactory<>), Lifestyle.Singleton);

        c.Register<IDefaultValueProvider<UInt16>>(() =>
            new DefaultValueProvider<UInt16>(() => 0),
            Lifestyle.Singleton);
        c.Register<IDefaultValueProvider<DateTime>>(() =>
            new DefaultValueProvider<DateTime>(() => DateTime.Now),
            Lifestyle.Singleton);

        RegisterLevel1DoubleArgFactory(c, typeof(ISelectInputGroupModel<,>), typeof(SelectInputGroupModelFactory<,>));
        RegisterLevel1DoubleArgFactory(c, typeof(ISelectInputModel<,>), typeof(SelectInputModelFactory<,>));
        RegisterLevel1DoubleArgFactory(c, typeof(IInputGroupModel<,>), typeof(InputGroupModelFactory<,>));
        RegisterLevel1DoubleArgFactory(c, typeof(IInputModel<,>), typeof(InputModelFactory<,>));

        c.RegisterConditional(
            typeof(IDynamicMultiControlModel<>),
            typeof(DynamicMultiControlModel<>),
            c => !c.Handled);
        c.Register(typeof(IDynamicMultiControlItemModelFactory<>), typeof(DynamicMultiControlItemModelFactory<>), Lifestyle.Singleton);
        c.Register<IFactory<IButtonModel>, ButtonModelFactory>(Lifestyle.Singleton);

        c.Register<IDisposeStrategy<IAgeModel>, DisposingStrategy<IAgeModel>>(Lifestyle.Singleton);
        c.RegisterConditional(
            typeof(IDisposeStrategy<>),
            typeof(NonDisposingStrategy<>),
            Lifestyle.Singleton,
            c => !c.Handled);

        c.Register<IAgeModel, AgeModel>();
        c.GetRegistration(typeof(IAgeModel))!.Registration
            .SuppressDiagnosticWarning(
                DiagnosticType.DisposableTransientComponent,
                "A disposing strategy has been injected alongside this transient service.");

        c.Register<IEqualityComparer<IToastModel>, ToastModelEqualityComparer>(Lifestyle.Singleton);
        c.Register<IToastModelFactory, ToastModelFactory>(Lifestyle.Singleton);
        c.Register<IToastsModel>(() =>
        {
            var children = c.GetAllInstances<IToastsModel>();
            var result = ToastsCompositeModel.Create(children);

            return result;
        }, Lifestyle.Scoped);
        c.Collection.Register<IToastsModel>(new[]
        {
                    typeof(ToastsErrorModel)
            }, Lifestyle.Scoped);
        c.Register<IObserver<Exception>, ToastsErrorModel>(Lifestyle.Scoped);
        c.RegisterConditional(
            typeof(IObserver<>),
            typeof(NullObserver<>),
            Lifestyle.Scoped,
            c => !c.Handled);

        c.Register<IButtonModel, ButtonModel>();

        c.Register<IDefaultValueProvider<Decimal>>(() =>
        {
            var result = new DefaultValueProvider<Decimal>(() => 0);
            return result;
        }, Lifestyle.Singleton);
        c.Register<IDefaultValueProvider<String>>(() =>
        {
            var result = new DefaultValueProvider<String>(() => String.Empty);
            return result;
        }, Lifestyle.Singleton);
        c.Register(typeof(IInputGroupModel<,>), typeof(InputGroupModel<,>));

        c.Register(typeof(IMultiControlModel<>), typeof(MultiControlModel<>));
    });

    /// <summary>
    /// <para>Registers factories that follow the following type signature constraints:</para>
    /// <para>Service Type := <c>IFactory{TProduct}</c></para>
    /// <para><paramref name="openProductType"/> := <c>TProduct</c></para>
    /// <para><c>TProduct</c> := <c>T{T1, T2}</c></para>
    /// <para><paramref name="openFactoryImplType"/> := <c>TFactoryImpl</c></para>
    /// <para><c>TFactoryImpl</c> := <c>TImpl{T1, T2}</c></para>
    /// <para><c>TFactoryImpl</c> := <c>IFactory{TProduct}</c></para>
    /// <para>Such that an implementation of <c>IFactory{TProduct}</c> may be constructed by supplying 
    /// the generic arguments <c>T1</c> and <c>T2</c> to the open generic type <c>TFactoryImpl</c> where
    /// <c>T1</c> is the first generic argument of the constructed generic type <c>TProduct</c> and <c>T2</c> the second one.</para>
    /// </summary>
    private static void RegisterLevel1DoubleArgFactory(Container c, Type openProductType, Type openFactoryImplType)
    {
        //register generic InputModelFactory<,>
        c.RegisterConditional(
            typeof(IFactory<>),
            c =>
            {
                //must be productType
                var productType = c.ServiceType.GenericTypeArguments[0];
                //obtain T1
                var arg1 = productType.GenericTypeArguments[0];
                //obtain T2
                var arg2 = productType.GenericTypeArguments[1];
                //construct impl type
                var factoryType = openFactoryImplType.MakeGenericType(arg1, arg2);

                return factoryType;
            },
            Lifestyle.Singleton,
            c =>
            {
                if(c.Handled)
                    return false; //already handled.

                var productType = c.ServiceType.GenericTypeArguments[0];
                if(!productType.IsGenericType)
                    return false; //cannot be T<T1,T2>

                var genericDefinition = productType.GetGenericTypeDefinition();
                if(!genericDefinition.IsAssignableTo(openProductType))
                    return false; //not assignable => different base type

                if(genericDefinition.GetGenericArguments().Length != 2)
                    return false; //contains incorrect amount of parameters

                return true; //assignable to openProductType
            });
    }
}
