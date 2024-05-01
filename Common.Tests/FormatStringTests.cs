namespace RhoMicro.ApplicationFramework.Common.Tests;
using System;
using System.Linq;

/// <summary>
/// Contains tests form format strings.
/// </summary>
public class FormatStringTests
{
    /// <summary>
    /// Data for <see cref="ParsesSource(String, FormatString)"/>
    /// </summary>
    public static Object[][] ParsesSourceData =>
        [
            new Object[]{
                "Qui velit repellendus {ad}. Facilis {iusto }rerum rem qui est {incidunt}",
                new FormatStringBuilder()
                    .AddLiteral("Qui velit repellendus ")
                    .AddParameter("ad")
                    .AddLiteral(". Facilis ")
                    .AddParameter("iusto ")
                    .AddLiteral("rerum rem qui est ")
                    .AddParameter("incidunt")
                    .Build()
            },
            [
                "c}orp{oris vit}ae. Vo{luptas ea aut omnis maiores",
                new FormatStringBuilder()
                    .AddLiteral("c}orp")
                    .AddParameter("oris vit")
                    .AddLiteral("ae. Vo{luptas ea aut omnis maiores")
                    .Build()
            ],
            [
                "{li}bero as{sumen}da magni ratione. Amet {sunt} illum aut dolor et q}ui.",
                new FormatStringBuilder()
                    .AddParameter("li")
                    .AddLiteral("bero as")
                    .AddParameter("sumen")
                    .AddLiteral("da magni ratione. Amet ")
                    .AddParameter("sunt")
                    .AddLiteral(" illum aut dolor et q}ui.")
                    .Build()
            ],
            [
                "{Voluptate} ipsam {et}. {Odio}",
                new FormatStringBuilder()
                    .AddParameter("Voluptate")
                    .AddLiteral(" ipsam ")
                    .AddParameter("et")
                    .AddLiteral(". ")
                    .AddParameter("Odio")
                    .Build()
            ],
            [
                "{li}bero as{sumen}da magni {sumen} ratione. Amet {sunt} illum {sunt}au{sunt}t dolor et q}ui.",
                new FormatStringBuilder()
                    .AddParameter("li")
                    .AddLiteral("bero as")
                    .AddParameter("sumen")
                    .AddLiteral("da magni ")
                    .AddParameter("sumen")
                    .AddLiteral(" ratione. Amet ")
                    .AddParameter("sunt")
                    .AddLiteral(" illum ")
                    .AddParameter("sunt")
                    .AddLiteral("au")
                    .AddParameter("sunt")
                    .AddLiteral("t dolor et q}ui.")
                    .Build()
            ]
        ];

    /// <summary>
    /// Data for <see cref="SubstitutesCorrectly(String, FormatStringArgument[], String)"/>
    /// </summary>
    public static Object[][] SubstitutesCorrectlyData =>
        [
            new Object[]{
                "Qui velit repellendus {ad}. Facilis {iusto }rerum rem qui est {incidunt}",
                new FormatStringArgument[]{("ad", "da"), ("incidunt", "foo"), ("iusto ", "bar")},
                "Qui velit repellendus da. Facilis barrerum rem qui est foo"
            },
            [
                "c}orp{oris vit}ae. Vo{luptas ea aut omnis maiores",
                new FormatStringArgument[]{("oris vit", "foobar")},
                "c}orpfoobarae. Vo{luptas ea aut omnis maiores"
            ],
            [
                "{li}bero as{sumen}da magni ratione. Amet {sunt} illum aut dolor et q}ui.",
                new FormatStringArgument[]{("li", "foo"),("sunt", "bar"), ("sumen", "foo")},
                "foobero asfooda magni ratione. Amet bar illum aut dolor et q}ui."
            ],
            [
                "{Voluptate} ipsam {et}. {Odio}",
                new FormatStringArgument[]{( "Voluptate", "baz" ),("Odio", "foobar"), ("et", "far") },
                "baz ipsam far. foobar"
            ],
            [
                "{li}bero as{sumen}da magni {sumen} ratione. Amet {sunt} illum {sunt}au{sunt}t dolor et q}ui.",
                new FormatStringArgument[]{("sunt","ssss"),("sumen", "foobar"), ("li", "foo")},
                "foobero asfoobarda magni foobar ratione. Amet ssss illum ssssausssst dolor et q}ui."
            ]
        ];
    /// <summary>
    /// Asserts that <see cref="FormatString.ToString(FormatStringArgument[])"/> will correctly substitute arguments for parameters.
    /// </summary>
    [Theory]
    [MemberData(nameof(SubstitutesCorrectlyData))]
    public void SubstitutesCorrectly(String source, FormatStringArgument[] arguments, String expected)
    {
        FormatString parsed = source;

        var actual = parsed.ToString(arguments);

        Assert.Equal(expected, actual);
    }
    /// <summary>
    /// Asserts that a parsed source will result in the correct format string.
    /// </summary>
    [Theory]
    [MemberData(nameof(ParsesSourceData))]
    public void ParsesSource(String source, FormatString expected)
    {
        FormatString actual = source;

        var areSequenceEqual = actual.Parts.SequenceEqual(expected.Parts);

        Assert.True(areSequenceEqual);
    }
}
