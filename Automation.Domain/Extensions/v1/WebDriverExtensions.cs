using OpenQA.Selenium;

namespace Automation.Alura.Domain.Extensions.v1;

public static class WebDriverExtensions
{
    public static async Task<IWebElement?> WaitFindElement(this IWebDriver driver, By by, CancellationToken cancellationToken = default)
    {
        IWebElement? element = null;

        var remainingAttempts = 3;

        while (remainingAttempts > 0)
        {
            try
            {
                element = driver.FindElement(by);

                break;
            }
            catch (Exception)
            {
                remainingAttempts--;

                await Task.Delay(300, cancellationToken);
            }
        }

        return element;
    }

    public static async Task<IWebElement?> WaitFindElement(this IWebElement elementRoot, By by, CancellationToken cancellationToken = default)
    {
        IWebElement? element = null;

        var remainingAttempts = 3;

        while (remainingAttempts > 0)
        {
            try
            {
                element = elementRoot.FindElement(by);

                break;
            }
            catch (Exception)
            {
                remainingAttempts--;

                await Task.Delay(300, cancellationToken);
            }
        }

        return element;
    }

    public static async Task<IEnumerable<IWebElement>> WaitFindElements(this IWebDriver driver, By by, CancellationToken cancellationToken = default)
    {
        IEnumerable<IWebElement> elements = new List<IWebElement>();

        var remainingAttempts = 3;

        while (remainingAttempts > 0)
        {
            try
            {
                elements = driver.FindElements(by);

                break;
            }
            catch (Exception)
            {
                remainingAttempts--;

                await Task.Delay(300, cancellationToken);
            }
        }

        return elements;
    }

    public static async Task<IEnumerable<IWebElement>> WaitFindElements(this IWebElement elementRoot, By by, CancellationToken cancellationToken = default)
    {
        IEnumerable<IWebElement> elements = new List<IWebElement>();

        var remainingAttempts = 3;

        while (remainingAttempts > 0)
        {
            try
            {
                elements = elementRoot.FindElements(by);

                break;
            }
            catch (Exception)
            {
                remainingAttempts--;

                await Task.Delay(300, cancellationToken);
            }
        }

        return elements;
    }
}