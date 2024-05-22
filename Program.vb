Imports CsvHelper
Imports System.Globalization
Imports System.IO
Imports HtmlAgilityPack

Public Module Program
    ' Sub Main()
    '     Dim web As New HtmlWeb()
    '     Dim document = web.Load("https://scrapeme.live/shop/")

    '     Console.WriteLine(document.DocumentNode.OuterHtml)
    ' End Sub

    ' Segundo exemplo
    ' Sub Main()
    '     Dim web As New HtmlWeb()
    '     Dim document = web.Load("https://scrapeme.live/shop/")
    '     Dim productHTMLElement = document.DocumentNode.QuerySelector("li.product")
        
    '     Dim name = HtmlEntity.DeEntitize(productHTMLElement.QuerySelector("h2").InnerText)
    '     Dim url = HtmlEntity.DeEntitize(productHTMLElement.QuerySelector("a").Attributes("href").value)
    '     Dim image = HtmlEntity.DeEntitize(productHTMLElement.QuerySelector("img").Attributes("src").value)
    '     Dim price = HtmlEntity.DeEntitize(productHTMLElement.QuerySelector(".price").InnerText)

    '     Console.WriteLine("Product URL: " & url)
    '     Console.WriteLine("Product Image: " & image)
    '     Console.WriteLine("Product Name: " & name)
    '     Console.WriteLine("Product Price: " & price)
    ' End Sub

    ' Terceiro exemplo iterando sobre uma lista de produtos
    ' define a custom class for the data to scrape
    Public Class Product
        Public Property Url As String
        Public Property Image As String
        Public Property Name As String
        Public Property Price As String
    End Class

    Sub Main()
        ' initialize the HAP HTTP client
        Dim web As New HtmlWeb()
        ' connect to target page
        Dim document = web.Load("https://scrapeme.live/shop/")
        ' where to store the scraped data
        Dim products As New List(Of Product)()
        ' select all HTML product nodes
        Dim productHTMLElements = document.DocumentNode.QuerySelectorAll("li.product")

        ' iterate over the list of product HTML elements
        For Each productHTMLElement In productHTMLElements
            ' scraping logis
            Dim name = HtmlEntity.DeEntitize(productHTMLElement.QuerySelector("h2").InnerText)
            Dim url = HtmlEntity.DeEntitize(productHTMLElement.QuerySelector("a").Attributes("href").value)
            Dim image = HtmlEntity.DeEntitize(productHTMLElement.QuerySelector("img").Attributes("src").value)
            Dim price = HtmlEntity.DeEntitize(productHTMLElement.QuerySelector(".price").InnerText)

            ' instantiate a new Product object with the scraped data
            ' and add it to the list
            Dim product = New Product with {
                .Url = url,
                .Image = image,
                .Name = name,
                .Price = price
            }
            products.Add(product)
        Next

        ' log the scraped data in the terminal
        For Each product In products
            Console.WriteLine("Product URL: " & product.url)
            Console.WriteLine("Product Image: " & product.image)
            Console.WriteLine("Product Name: " & product.name)
            Console.WriteLine("Product Price: " & product.price)
            Console.WriteLine()
        Next

        ' export the scradped data to csv
        Using writer As New StreamWriter("products.csv")
            Using csv As New CsvWriter(writer, CultureInfo.InvariantCulture)
                csv.WriteRecords(products)
            End Using
        End Using
    End Sub
End Module