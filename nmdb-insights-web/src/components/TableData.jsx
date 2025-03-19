"use client";

import { useState } from "react";

const data = [
  {
    date: "Mar 10",
    day: "Monday",
    dayNumber: 69,
    topGross: 4422812,
    yearChange: -65.6,
    weekChange: 31.7,
    releases: 23,
    topRelease: "Mickey 17",
    gross: 1581557,
  },
  {
    date: "Mar 9",
    day: "Sunday",
    dayNumber: 68,
    topGross: 12849106,
    yearChange: -35.8,
    weekChange: -2.4,
    releases: 46,
    topRelease: "Mickey 17",
    gross: 4574026,
  },
  {
    date: "Mar 8",
    day: "Saturday",
    dayNumber: 67,
    topGross: 20012325,
    yearChange: 27.6,
    weekChange: 0.2,
    releases: 46,
    topRelease: "Mickey 17",
    gross: 6702116,
  },
  {
    date: "Mar 7",
    day: "Friday",
    dayNumber: 66,
    topGross: 15683528,
    yearChange: 380.2,
    weekChange: 27.4,
    releases: 46,
    topRelease: "Mickey 17",
    gross: 7726710,
  },
  {
    date: "Mar 6",
    day: "Thursday",
    dayNumber: 65,
    topGross: 3265871,
    yearChange: -9.0,
    weekChange: -15.5,
    releases: 44,
    topRelease: "Captain America: Brave New World",
    gross: 932577,
  },
  {
    date: "Mar 5",
    day: "Wednesday",
    dayNumber: 64,
    topGross: 3588110,
    yearChange: -32.9,
    weekChange: -12.4,
    releases: 45,
    topRelease: "Captain America: Brave New World",
    gross: 1021083,
  },
  {
    date: "Mar 4",
    day: "Tuesday",
    dayNumber: 63,
    topGross: 5344156,
    yearChange: 59.2,
    weekChange: -22.8,
    releases: 45,
    topRelease: "Captain America: Brave New World",
    gross: 1551038,
  },
  {
    date: "Mar 3",
    day: "Monday",
    dayNumber: 62,
    topGross: 3357357,
    yearChange: -74.5,
    weekChange: -25.0,
    releases: 43,
    topRelease: "Captain America: Brave New World",
    gross: 1032239,
  },
  {
    date: "Mar 2",
    day: "Sunday",
    dayNumber: 61,
    topGross: 13161926,
    yearChange: -34.1,
    weekChange: -29.9,
    releases: 47,
    topRelease: "Captain America: Brave New World",
    gross: 4446456,
  },
  {
    date: "Mar 1",
    day: "Saturday",
    dayNumber: 60,
    topGross: 19963984,
    yearChange: 62.2,
    weekChange: -32.5,
    releases: 47,
    topRelease: "Captain America: Brave New World",
    gross: 6845635,
  },
];

// Function to format currency and percentage values
const formatValue = (value, type) => {
  if (type === "currency") {
    return `$${value.toLocaleString()}`;
  }
  if (type === "percentage") {
    return `${value > 0 ? "+" : ""}${value}%`;
  }
  return value;
};

// Function to sort data based on column and direction
const sortData = (data, column, direction) => {
  return [...data].sort((a, b) => {
    const aValue = a[column];
    const bValue = b[column];

    if (typeof aValue === "string" && typeof bValue === "string") {
      return direction === "asc"
        ? aValue.localeCompare(bValue)
        : bValue.localeCompare(aValue);
    }

    return direction === "asc" ? aValue - bValue : bValue - aValue;
  });
};

function TableData() {
  const [sortColumn, setSortColumn] = useState("date");
  const [sortDirection, setSortDirection] = useState("asc");

  // Handle sorting when a column header is clicked
  const handleSort = (column) => {
    if (sortColumn === column) {
      setSortDirection(sortDirection === "asc" ? "desc" : "asc");
    } else {
      setSortColumn(column);
      setSortDirection("asc");
    }
  };

  // Get sorted data based on current sort column and direction
  const sortedData = sortData(data, sortColumn, sortDirection);

  return (
    <div className="w-full max-w-[1200px] mx-auto">
      <div className="mb-6">
        <h1 className="text-2xl font-bold mb-4">Daily Box Office For 2025</h1>
      </div>

      <div className="border border-gray-200 rounded-lg overflow-x-auto">
        <table className="min-w-full divide-y divide-gray-200">
          <thead className="bg-gray-50">
            <tr>
              <th
                scope="col"
                className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider cursor-pointer w-[150px]"
                onClick={() => handleSort("date")}
              >
                Date
              </th>
              <th
                scope="col"
                className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider"
              >
                Day
              </th>
              <th
                scope="col"
                className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider"
              >
                Day #
              </th>
              <th
                scope="col"
                className="px-6 py-3 text-right text-xs font-medium text-gray-500 uppercase tracking-wider cursor-pointer"
                onClick={() => handleSort("topGross")}
              >
                Top 10 Gross
              </th>
              <th
                scope="col"
                className="px-6 py-3 text-right text-xs font-medium text-gray-500 uppercase tracking-wider"
              >
                %± YD
              </th>
              <th
                scope="col"
                className="px-6 py-3 text-right text-xs font-medium text-gray-500 uppercase tracking-wider"
              >
                %± LW
              </th>
              <th
                scope="col"
                className="px-6 py-3 text-right text-xs font-medium text-gray-500 uppercase tracking-wider cursor-pointer"
                onClick={() => handleSort("releases")}
              >
                Releases
              </th>
              <th
                scope="col"
                className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider"
              >
                #1 Release
              </th>
              <th
                scope="col"
                className="px-6 py-3 text-right text-xs font-medium text-gray-500 uppercase tracking-wider cursor-pointer w-[150px]"
                onClick={() => handleSort("gross")}
              >
                Gross
              </th>
            </tr>
          </thead>
          <tbody className="bg-white divide-y divide-gray-200">
            {sortedData.map((row, index) => (
              <tr key={index} className="hover:bg-gray-50">
                <td className="px-6 py-4 whitespace-nowrap w-[150px]">
                  <a href="#" className="text-blue-600 hover:underline">
                    {row.date}
                  </a>
                </td>
                <td className="px-6 py-4 whitespace-nowrap">{row.day}</td>
                <td className="px-6 py-4 whitespace-nowrap">{row.dayNumber}</td>
                <td className="px-6 py-4 whitespace-nowrap text-right">
                  {formatValue(row.topGross, "currency")}
                </td>
                <td
                  className={`px-6 py-4 whitespace-nowrap text-right ${
                    row.yearChange > 0 ? "text-green-600" : "text-red-600"
                  }`}
                >
                  {formatValue(row.yearChange, "percentage")}
                </td>
                <td
                  className={`px-6 py-4 whitespace-nowrap text-right ${
                    row.weekChange > 0 ? "text-green-600" : "text-red-600"
                  }`}
                >
                  {formatValue(row.weekChange, "percentage")}
                </td>
                <td className="px-6 py-4 whitespace-nowrap text-right">
                  {row.releases}
                </td>
                <td className="px-6 py-4 whitespace-nowrap">
                  <a href="#" className="text-blue-600 hover:underline">
                    {row.topRelease}
                  </a>
                </td>
                <td className="px-6 py-4 whitespace-nowrap text-right w-[150px]">
                  {formatValue(row.gross, "currency")}
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </div>
  );
}

export default TableData;
