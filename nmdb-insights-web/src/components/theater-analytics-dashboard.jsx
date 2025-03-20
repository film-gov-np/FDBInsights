"use client";

import { useState, useEffect } from "react";

import { TheaterHeader } from "./theater-header";

import { TimeFrameTabs } from "./time-frame-tabs";

import { DateRangeFilter } from "./data-range-filter";

import { MetricsCards } from "./metrics-cards";

import { RevenueChart } from "./revenue-chart";

import { TicketSalesChart } from "./ticket-sales-chart";

import { OccupancyChart } from "./occupancy-chart";

import { MoviePerformanceTable } from "./movie-performance-table";

import { theaterData, refreshData } from "./mock-data";

export default function TheaterAnalyticsDashboard() {
  const [selectedTimeFrame, setSelectedTimeFrame] = useState("daily");
  const [selectedTheater, setSelectedTheater] = useState("qfx");
  const [selectedScreen, setSelectedScreen] = useState("All Screens");
  const [data, setData] = useState(theaterData.qfx.daily["All Screens"]);
  const [isLoading, setIsLoading] = useState(false);

  // Get the appropriate data based on selected theater, time frame, and screen
  useEffect(() => {
    updateData();
  }, [selectedTimeFrame, selectedTheater, selectedScreen]);

  const updateData = () => {
    try {
      const newData =
        theaterData[selectedTheater][selectedTimeFrame][selectedScreen];
      setData(newData);
    } catch (error) {
      console.error("Error updating data:", error);
      // Fallback to default data if there's an error
      setData(theaterData.qfx.daily["All Screens"]);
    }
  };

  // Handle refresh button click
  const handleRefresh = () => {
    setIsLoading(true);

    // Simulate API call with a delay
    setTimeout(() => {
      // Generate refreshed data with slight variations
      const refreshedData = refreshData(data);
      setData(refreshedData);
      setIsLoading(false);
    }, 800); // Simulate network delay
  };

  // Handle theater change
  const handleTheaterChange = (theater) => {
    setSelectedTheater(theater);
  };

  // Handle time frame change
  const handleTimeFrameChange = (timeFrame) => {
    setSelectedTimeFrame(timeFrame);
  };

  // Handle screen change
  const handleScreenChange = (screen) => {
    setSelectedScreen(screen);
  };

  return (
    <div className="container mx-auto p-4 max-w-7xl ">
      <TheaterHeader
        onRefresh={handleRefresh}
        onTheaterChange={handleTheaterChange}
        selectedTheater={selectedTheater}
        isLoading={isLoading}
      />

      <div className="mb-6">
        <h2 className="text-2xl font-bold mb-4">Theater Analytics</h2>
        <div className="flex justify-between items-center mb-4">
          <TimeFrameTabs
            selectedTimeFrame={selectedTimeFrame}
            onTimeFrameChange={handleTimeFrameChange}
          />
          <DateRangeFilter
            selectedScreen={selectedScreen}
            onScreenChange={handleScreenChange}
            dateRange={data.dateRange}
          />
        </div>

        <div
          className={`transition-opacity duration-300 ${
            isLoading ? "opacity-50" : "opacity-100"
          }`}
        >
          <MetricsCards metrics={data.metrics} />

          <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6 mb-8">
            <RevenueChart
              data={data.revenueData}
              title={`${
                selectedTimeFrame.charAt(0).toUpperCase() +
                selectedTimeFrame.slice(1)
              } Revenue Trend`}
            />
            <TicketSalesChart
              data={data.ticketData}
              title={`${
                selectedTimeFrame.charAt(0).toUpperCase() +
                selectedTimeFrame.slice(1)
              } Ticket Sales`}
            />
            <OccupancyChart
              data={data.occupancyData}
              title="Average Occupancy"
            />
          </div>

          <MoviePerformanceTable data={data.movieData} />
        </div>
      </div>
    </div>
  );
}
