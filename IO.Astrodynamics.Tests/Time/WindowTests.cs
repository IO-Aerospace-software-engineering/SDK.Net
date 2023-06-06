using System;
using IO.Astrodynamics.Models.Time;
using Xunit;

namespace IO.Astrodynamics.Models.Tests.Time;
public class WindowTests
{
    [Fact]
    void CreateWindowFromDates()
    {
        Window w = new Window(new DateTime(2021, 01, 02), new DateTime(2021, 01, 03));
        Assert.Equal(w.StartDate, new DateTime(2021, 01, 02));
        Assert.Equal(w.EndDate, new DateTime(2021, 01, 03));
    }

    [Fact]
    void CreateWindowFromReversedDates()
    {
        Window w = new Window(new DateTime(2021, 01, 03), new DateTime(2021, 01, 02));
        Assert.Equal(w.StartDate, new DateTime(2021, 01, 02));
        Assert.Equal(w.EndDate, new DateTime(2021, 01, 03));
    }

    [Fact]
    void CreateWindowFromLength()
    {
        Window w = new Window(new DateTime(2021, 01, 02), TimeSpan.FromDays(1.0));
        Assert.Equal(w.StartDate, new DateTime(2021, 01, 02));
        Assert.Equal(w.EndDate, new DateTime(2021, 01, 03));
    }

    [Fact]
    void MergeWindows()
    {
        Window w1 = new Window(new DateTime(2021, 01, 02), new DateTime(2021, 01, 10));
        Window w2 = new Window(new DateTime(2021, 01, 04), new DateTime(2021, 01, 12));
        Window res = w1.Merge(w2);

        Assert.Equal(res, new Window(new DateTime(2021, 01, 02), new DateTime(2021, 01, 12)));
    }

    [Fact]
    void WindowsIntersect()
    {
        Window w1 = new Window(new DateTime(2021, 01, 02), new DateTime(2021, 01, 10));
        Window w2 = new Window(new DateTime(2021, 01, 04), new DateTime(2021, 01, 12));
        bool res = w1.Intersects(w2);
        Assert.True(res);
    }

    [Fact]
    void WindowsDontIntersect()
    {
        Window w1 = new Window(new DateTime(2021, 01, 02), new DateTime(2021, 01, 10));
        Window w2 = new Window(new DateTime(2021, 01, 11), new DateTime(2021, 01, 12));
        bool res = w1.Intersects(w2);
        Assert.False(res);
    }

    [Fact]
    void DateTimeIntersect()
    {
        Window w1 = new Window(new DateTime(2021, 01, 02), new DateTime(2021, 01, 10));
        DateTime dt = new DateTime(2021, 01, 10);
        bool res = w1.Intersects(dt);
        Assert.True(res);
    }

    [Fact]
    void DateTimeDoesntIntersect()
    {
        Window w1 = new Window(new DateTime(2021, 01, 02), new DateTime(2021, 01, 10));
        DateTime dt = new DateTime(2021, 01, 11);
        bool res = w1.Intersects(dt);
        Assert.False(res);
    }

    [Fact]
    void WindowsGetIntersection()
    {
        Window w1 = new Window(new DateTime(2021, 01, 02), new DateTime(2021, 01, 10));
        Window w2 = new Window(new DateTime(2021, 01, 04), new DateTime(2021, 01, 12));
        Assert.Equal(w1.GetIntersection(w2), new Window(new DateTime(2021, 01, 04), new DateTime(2021, 01, 10)));
    }
}