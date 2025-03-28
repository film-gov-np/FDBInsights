// Mock data for different theaters, time frames, and screens

// Helper function to create screen-specific data
const createScreenData = (baseData, screenNumber) => {
  // Adjust values based on screen number to create variation
  const multiplier = 0.7 + screenNumber * 0.1; // Screen 1: 0.8, Screen 2: 0.9, etc.

  return {
    ...baseData,
    metrics: {
      boxOffice: {
        value: `NPR ${Math.floor(
          Number.parseInt(
            baseData.metrics.boxOffice.value.replace(/[^0-9]/g, "")
          ) * multiplier
        ).toLocaleString()}`,
        change: +(baseData.metrics.boxOffice.change * multiplier).toFixed(1),
      },
      ticketsSold: {
        value: Math.floor(
          Number.parseInt(
            baseData.metrics.ticketsSold.value.replace(/[^0-9]/g, "")
          ) * multiplier
        ).toLocaleString(),
        change: +(baseData.metrics.ticketsSold.change * multiplier).toFixed(1),
      },
      avgTicketPrice: {
        value: `NPR ${(
          Number.parseFloat(
            baseData.metrics.avgTicketPrice.value.replace(/[^0-9.]/g, "")
          ) *
          (1 + screenNumber * 0.02)
        ).toFixed(2)}`,
        change: +(baseData.metrics.avgTicketPrice.change * multiplier).toFixed(
          1
        ),
      },
      avgOccupancy: {
        value: `${Math.min(
          100,
          Math.floor(
            Number.parseInt(
              baseData.metrics.avgOccupancy.value.replace(/[^0-9]/g, "")
            ) * multiplier
          )
        )}%`,
        change: +(baseData.metrics.avgOccupancy.change * multiplier).toFixed(1),
      },
    },
    revenueData: baseData.revenueData.map((item) => ({
      ...item,
      value: Math.floor(item.value * multiplier),
    })),
    ticketData: baseData.ticketData.map((item) => ({
      ...item,
      value: Math.floor(item.value * multiplier),
    })),
    occupancyData: [
      {
        name: "Occupied",
        value: Math.min(
          100,
          Math.floor(baseData.occupancyData[0].value * multiplier)
        ),
      },
      {
        name: "Vacant",
        value:
          100 -
          Math.min(
            100,
            Math.floor(baseData.occupancyData[0].value * multiplier)
          ),
      },
    ],
    movieData: baseData.movieData.map((movie) => ({
      ...movie,
      gross: `NPR ${Math.floor(
        Number.parseInt(movie.gross.replace(/[^0-9]/g, "")) * multiplier
      ).toLocaleString()}`,
      change: movie.change.startsWith("+")
        ? `+${(
            Number.parseFloat(movie.change.replace(/[^0-9.]/g, "")) * multiplier
          ).toFixed(1)}%`
        : `-${(
            Number.parseFloat(movie.change.replace(/[^0-9.]/g, "")) * multiplier
          ).toFixed(1)}%`,
      shows: Math.max(1, Math.floor(movie.shows * multiplier)),
      avgPerShow: `NPR ${Math.floor(
        Number.parseInt(movie.avgPerShow.replace(/[^0-9]/g, "")) *
          (1 + screenNumber * 0.05)
      ).toLocaleString()}`,
      occupancy: `${Math.min(
        100,
        Math.floor(
          Number.parseInt(movie.occupancy.replace(/[^0-9]/g, "")) * multiplier
        )
      )}%`,
    })),
  };
};

// Create data for each screen
const createScreensData = (baseData) => {
  return {
    "All Screens": baseData,
    "Screen 1": createScreenData(baseData, 1),
    "Screen 2": createScreenData(baseData, 2),
    "Screen 3": createScreenData(baseData, 3),
    "Screen 4": createScreenData(baseData, 4),
  };
};

// Create data for each time frame and screen
const createTheaterData = (baseData) => {
  return {
    daily: createScreensData(baseData.daily),
    weekend: createScreensData(baseData.weekend),
    weekly: createScreensData(baseData.weekly),
    monthly: createScreensData(baseData.monthly),
  };
};

// Base data for QFX Cinemas
const qfxBaseData = {
  daily: {
    dateRange: "Mar 7, 2025 - Mar 13, 2025",
    metrics: {
      boxOffice: { value: "NPR 168,000", change: 5.0 },
      ticketsSold: { value: "1,250", change: 4.2 },
      avgTicketPrice: { value: "NPR 134.40", change: 0.8 },
      avgOccupancy: { value: "72%", change: 2 },
    },
    revenueData: [
      { day: "Mar 7", value: 150000 },
      { day: "Mar 8", value: 180000 },
      { day: "Mar 9", value: 160000 },
      { day: "Mar 10", value: 140000 },
      { day: "Mar 11", value: 145000 },
      { day: "Mar 12", value: 155000 },
      { day: "Mar 13", value: 170000 },
    ],
    ticketData: [
      { day: "Mar 7", value: 1150 },
      { day: "Mar 8", value: 1350 },
      { day: "Mar 9", value: 1100 },
      { day: "Mar 10", value: 1050 },
      { day: "Mar 11", value: 1080 },
      { day: "Mar 12", value: 1200 },
      { day: "Mar 13", value: 1250 },
    ],
    occupancyData: [
      { name: "Occupied", value: 72 },
      { name: "Vacant", value: 28 },
    ],
    movieData: [
      {
        movie: "Himalayan Heights",
        gross: "NPR 45,000",
        change: "+5.2%",
        shows: 12,
        avgPerShow: "NPR 3,750",
        occupancy: "78%",
      },
      {
        movie: "Kathmandu Dreams",
        gross: "NPR 38,000",
        change: "-2.1%",
        shows: 10,
        avgPerShow: "NPR 3,800",
        occupancy: "72%",
      },
      {
        movie: "Mountain Melody",
        gross: "NPR 32,000",
        change: "+1.5%",
        shows: 8,
        avgPerShow: "NPR 4,000",
        occupancy: "75%",
      },
      {
        movie: "Village Tales",
        gross: "NPR 28,000",
        change: "-3.2%",
        shows: 7,
        avgPerShow: "NPR 4,000",
        occupancy: "70%",
      },
      {
        movie: "River's Journey",
        gross: "NPR 25,000",
        change: "+7.8%",
        shows: 6,
        avgPerShow: "NPR 4,166",
        occupancy: "68%",
      },
    ],
  },
  weekend: {
    dateRange: "Mar 8-9, 2025",
    metrics: {
      boxOffice: { value: "NPR 340,000", change: 12.5 },
      ticketsSold: { value: "2,450", change: 10.8 },
      avgTicketPrice: { value: "NPR 138.80", change: 1.5 },
      avgOccupancy: { value: "85%", change: 8 },
    },
    revenueData: [
      { day: "Mar 8", value: 180000 },
      { day: "Mar 9", value: 160000 },
    ],
    ticketData: [
      { day: "Mar 8", value: 1350 },
      { day: "Mar 9", value: 1100 },
    ],
    occupancyData: [
      { name: "Occupied", value: 85 },
      { name: "Vacant", value: 15 },
    ],
    movieData: [
      {
        movie: "Himalayan Heights",
        gross: "NPR 95,000",
        change: "+12.5%",
        shows: 8,
        avgPerShow: "NPR 11,875",
        occupancy: "90%",
      },
      {
        movie: "Kathmandu Dreams",
        gross: "NPR 82,000",
        change: "+8.2%",
        shows: 6,
        avgPerShow: "NPR 13,667",
        occupancy: "88%",
      },
      {
        movie: "Mountain Melody",
        gross: "NPR 68,000",
        change: "+15.3%",
        shows: 5,
        avgPerShow: "NPR 13,600",
        occupancy: "85%",
      },
      {
        movie: "Village Tales",
        gross: "NPR 55,000",
        change: "+10.2%",
        shows: 4,
        avgPerShow: "NPR 13,750",
        occupancy: "82%",
      },
      {
        movie: "River's Journey",
        gross: "NPR 40,000",
        change: "+18.5%",
        shows: 3,
        avgPerShow: "NPR 13,333",
        occupancy: "80%",
      },
    ],
  },
  weekly: {
    dateRange: "Mar 7-13, 2025",
    metrics: {
      boxOffice: { value: "NPR 1,100,000", change: 8.2 },
      ticketsSold: { value: "8,180", change: 7.5 },
      avgTicketPrice: { value: "NPR 134.50", change: 0.7 },
      avgOccupancy: { value: "68%", change: 3 },
    },
    revenueData: [
      { day: "Week 10", value: 980000 },
      { day: "Week 11", value: 1100000 },
    ],
    ticketData: [
      { day: "Week 10", value: 7600 },
      { day: "Week 11", value: 8180 },
    ],
    occupancyData: [
      { name: "Occupied", value: 68 },
      { name: "Vacant", value: 32 },
    ],
    movieData: [
      {
        movie: "Himalayan Heights",
        gross: "NPR 320,000",
        change: "+8.5%",
        shows: 42,
        avgPerShow: "NPR 7,619",
        occupancy: "75%",
      },
      {
        movie: "Kathmandu Dreams",
        gross: "NPR 280,000",
        change: "+5.2%",
        shows: 35,
        avgPerShow: "NPR 8,000",
        occupancy: "70%",
      },
      {
        movie: "Mountain Melody",
        gross: "NPR 220,000",
        change: "+7.8%",
        shows: 28,
        avgPerShow: "NPR 7,857",
        occupancy: "72%",
      },
      {
        movie: "Village Tales",
        gross: "NPR 180,000",
        change: "+2.1%",
        shows: 24,
        avgPerShow: "NPR 7,500",
        occupancy: "65%",
      },
      {
        movie: "River's Journey",
        gross: "NPR 100,000",
        change: "+12.3%",
        shows: 21,
        avgPerShow: "NPR 4,762",
        occupancy: "60%",
      },
    ],
  },
  monthly: {
    dateRange: "Mar 2025",
    metrics: {
      boxOffice: { value: "NPR 4,500,000", change: 15.8 },
      ticketsSold: { value: "33,450", change: 12.3 },
      avgTicketPrice: { value: "NPR 134.50", change: 3.2 },
      avgOccupancy: { value: "65%", change: 5 },
    },
    revenueData: [
      { day: "Jan", value: 3800000 },
      { day: "Feb", value: 3900000 },
      { day: "Mar", value: 4500000 },
    ],
    ticketData: [
      { day: "Jan", value: 28500 },
      { day: "Feb", value: 29800 },
      { day: "Mar", value: 33450 },
    ],
    occupancyData: [
      { name: "Occupied", value: 65 },
      { name: "Vacant", value: 35 },
    ],
    movieData: [
      {
        movie: "Himalayan Heights",
        gross: "NPR 1,250,000",
        change: "+18.5%",
        shows: 160,
        avgPerShow: "NPR 7,813",
        occupancy: "72%",
      },
      {
        movie: "Kathmandu Dreams",
        gross: "NPR 980,000",
        change: "+12.8%",
        shows: 140,
        avgPerShow: "NPR 7,000",
        occupancy: "68%",
      },
      {
        movie: "Mountain Melody",
        gross: "NPR 850,000",
        change: "+15.2%",
        shows: 120,
        avgPerShow: "NPR 7,083",
        occupancy: "70%",
      },
      {
        movie: "Village Tales",
        gross: "NPR 780,000",
        change: "+10.5%",
        shows: 110,
        avgPerShow: "NPR 7,091",
        occupancy: "65%",
      },
      {
        movie: "River's Journey",
        gross: "NPR 640,000",
        change: "+20.1%",
        shows: 90,
        avgPerShow: "NPR 7,111",
        occupancy: "62%",
      },
    ],
  },
};

// Base data for FCube Cinemas
const fcubeBaseData = {
  daily: {
    dateRange: "Mar 7, 2025 - Mar 13, 2025",
    metrics: {
      boxOffice: { value: "NPR 142,000", change: 3.8 },
      ticketsSold: { value: "1,050", change: 2.5 },
      avgTicketPrice: { value: "NPR 135.20", change: 1.2 },
      avgOccupancy: { value: "65%", change: 1.5 },
    },
    revenueData: [
      { day: "Mar 7", value: 130000 },
      { day: "Mar 8", value: 155000 },
      { day: "Mar 9", value: 140000 },
      { day: "Mar 10", value: 125000 },
      { day: "Mar 11", value: 130000 },
      { day: "Mar 12", value: 135000 },
      { day: "Mar 13", value: 142000 },
    ],
    ticketData: [
      { day: "Mar 7", value: 950 },
      { day: "Mar 8", value: 1150 },
      { day: "Mar 9", value: 1050 },
      { day: "Mar 10", value: 920 },
      { day: "Mar 11", value: 980 },
      { day: "Mar 12", value: 1000 },
      { day: "Mar 13", value: 1050 },
    ],
    occupancyData: [
      { name: "Occupied", value: 65 },
      { name: "Vacant", value: 35 },
    ],
    movieData: [
      {
        movie: "Himalayan Heights",
        gross: "NPR 38,000",
        change: "+4.2%",
        shows: 10,
        avgPerShow: "NPR 3,800",
        occupancy: "70%",
      },
      {
        movie: "Kathmandu Dreams",
        gross: "NPR 32,000",
        change: "-1.5%",
        shows: 8,
        avgPerShow: "NPR 4,000",
        occupancy: "68%",
      },
      {
        movie: "Mountain Melody",
        gross: "NPR 28,000",
        change: "+2.1%",
        shows: 7,
        avgPerShow: "NPR 4,000",
        occupancy: "65%",
      },
      {
        movie: "Village Tales",
        gross: "NPR 24,000",
        change: "-2.8%",
        shows: 6,
        avgPerShow: "NPR 4,000",
        occupancy: "62%",
      },
      {
        movie: "River's Journey",
        gross: "NPR 20,000",
        change: "+5.5%",
        shows: 5,
        avgPerShow: "NPR 4,000",
        occupancy: "60%",
      },
    ],
  },
  weekend: {
    dateRange: "Mar 8-9, 2025",
    metrics: {
      boxOffice: { value: "NPR 295,000", change: 10.2 },
      ticketsSold: { value: "2,200", change: 8.5 },
      avgTicketPrice: { value: "NPR 134.10", change: 1.8 },
      avgOccupancy: { value: "78%", change: 6 },
    },
    revenueData: [
      { day: "Mar 8", value: 155000 },
      { day: "Mar 9", value: 140000 },
    ],
    ticketData: [
      { day: "Mar 8", value: 1150 },
      { day: "Mar 9", value: 1050 },
    ],
    occupancyData: [
      { name: "Occupied", value: 78 },
      { name: "Vacant", value: 22 },
    ],
    movieData: [
      {
        movie: "Himalayan Heights",
        gross: "NPR 82,000",
        change: "+10.5%",
        shows: 7,
        avgPerShow: "NPR 11,714",
        occupancy: "85%",
      },
      {
        movie: "Kathmandu Dreams",
        gross: "NPR 75,000",
        change: "+7.2%",
        shows: 6,
        avgPerShow: "NPR 12,500",
        occupancy: "80%",
      },
      {
        movie: "Mountain Melody",
        gross: "NPR 60,000",
        change: "+12.8%",
        shows: 5,
        avgPerShow: "NPR 12,000",
        occupancy: "78%",
      },
      {
        movie: "Village Tales",
        gross: "NPR 45,000",
        change: "+8.5%",
        shows: 4,
        avgPerShow: "NPR 11,250",
        occupancy: "75%",
      },
      {
        movie: "River's Journey",
        gross: "NPR 33,000",
        change: "+15.2%",
        shows: 3,
        avgPerShow: "NPR 11,000",
        occupancy: "72%",
      },
    ],
  },
  weekly: {
    dateRange: "Mar 7-13, 2025",
    metrics: {
      boxOffice: { value: "NPR 950,000", change: 6.5 },
      ticketsSold: { value: "7,100", change: 5.2 },
      avgTicketPrice: { value: "NPR 133.80", change: 1.2 },
      avgOccupancy: { value: "62%", change: 2.8 },
    },
    revenueData: [
      { day: "Week 10", value: 890000 },
      { day: "Week 11", value: 950000 },
    ],
    ticketData: [
      { day: "Week 10", value: 6750 },
      { day: "Week 11", value: 7100 },
    ],
    occupancyData: [
      { name: "Occupied", value: 62 },
      { name: "Vacant", value: 38 },
    ],
    movieData: [
      {
        movie: "Himalayan Heights",
        gross: "NPR 280,000",
        change: "+7.2%",
        shows: 38,
        avgPerShow: "NPR 7,368",
        occupancy: "68%",
      },
      {
        movie: "Kathmandu Dreams",
        gross: "NPR 240,000",
        change: "+4.5%",
        shows: 32,
        avgPerShow: "NPR 7,500",
        occupancy: "65%",
      },
      {
        movie: "Mountain Melody",
        gross: "NPR 190,000",
        change: "+6.2%",
        shows: 25,
        avgPerShow: "NPR 7,600",
        occupancy: "64%",
      },
      {
        movie: "Village Tales",
        gross: "NPR 150,000",
        change: "+1.8%",
        shows: 20,
        avgPerShow: "NPR 7,500",
        occupancy: "60%",
      },
      {
        movie: "River's Journey",
        gross: "NPR 90,000",
        change: "+10.5%",
        shows: 18,
        avgPerShow: "NPR 5,000",
        occupancy: "55%",
      },
    ],
  },
  monthly: {
    dateRange: "Mar 2025",
    metrics: {
      boxOffice: { value: "NPR 3,800,000", change: 12.5 },
      ticketsSold: { value: "28,500", change: 10.2 },
      avgTicketPrice: { value: "NPR 133.30", change: 2.1 },
      avgOccupancy: { value: "60%", change: 4 },
    },
    revenueData: [
      { day: "Jan", value: 3200000 },
      { day: "Feb", value: 3400000 },
      { day: "Mar", value: 3800000 },
    ],
    ticketData: [
      { day: "Jan", value: 24200 },
      { day: "Feb", value: 25800 },
      { day: "Mar", value: 28500 },
    ],
    occupancyData: [
      { name: "Occupied", value: 60 },
      { name: "Vacant", value: 40 },
    ],
    movieData: [
      {
        movie: "Himalayan Heights",
        gross: "NPR 1,050,000",
        change: "+15.2%",
        shows: 145,
        avgPerShow: "NPR 7,241",
        occupancy: "68%",
      },
      {
        movie: "Kathmandu Dreams",
        gross: "NPR 850,000",
        change: "+10.5%",
        shows: 125,
        avgPerShow: "NPR 6,800",
        occupancy: "65%",
      },
      {
        movie: "Mountain Melody",
        gross: "NPR 720,000",
        change: "+12.8%",
        shows: 110,
        avgPerShow: "NPR 6,545",
        occupancy: "62%",
      },
      {
        movie: "Village Tales",
        gross: "NPR 650,000",
        change: "+8.2%",
        shows: 100,
        avgPerShow: "NPR 6,500",
        occupancy: "58%",
      },
      {
        movie: "River's Journey",
        gross: "NPR 530,000",
        change: "+18.5%",
        shows: 85,
        avgPerShow: "NPR 6,235",
        occupancy: "55%",
      },
    ],
  },
};

// Base data for Big Movies
const bigBaseData = {
  daily: {
    dateRange: "Mar 7, 2025 - Mar 13, 2025",
    metrics: {
      boxOffice: { value: "NPR 125,000", change: 2.5 },
      ticketsSold: { value: "980", change: 1.8 },
      avgTicketPrice: { value: "NPR 127.55", change: 0.5 },
      avgOccupancy: { value: "58%", change: 1.2 },
    },
    revenueData: [
      { day: "Mar 7", value: 115000 },
      { day: "Mar 8", value: 135000 },
      { day: "Mar 9", value: 125000 },
      { day: "Mar 10", value: 110000 },
      { day: "Mar 11", value: 115000 },
      { day: "Mar 12", value: 120000 },
      { day: "Mar 13", value: 125000 },
    ],
    ticketData: [
      { day: "Mar 7", value: 900 },
      { day: "Mar 8", value: 1050 },
      { day: "Mar 9", value: 980 },
      { day: "Mar 10", value: 860 },
      { day: "Mar 11", value: 900 },
      { day: "Mar 12", value: 940 },
      { day: "Mar 13", value: 980 },
    ],
    occupancyData: [
      { name: "Occupied", value: 58 },
      { name: "Vacant", value: 42 },
    ],
    movieData: [
      {
        movie: "Himalayan Heights",
        gross: "NPR 32,000",
        change: "+3.5%",
        shows: 8,
        avgPerShow: "NPR 4,000",
        occupancy: "65%",
      },
      {
        movie: "Kathmandu Dreams",
        gross: "NPR 28,000",
        change: "-0.8%",
        shows: 7,
        avgPerShow: "NPR 4,000",
        occupancy: "62%",
      },
      {
        movie: "Mountain Melody",
        gross: "NPR 25,000",
        change: "+1.2%",
        shows: 6,
        avgPerShow: "NPR 4,167",
        occupancy: "60%",
      },
      {
        movie: "Village Tales",
        gross: "NPR 22,000",
        change: "-1.5%",
        shows: 5,
        avgPerShow: "NPR 4,400",
        occupancy: "55%",
      },
      {
        movie: "River's Journey",
        gross: "NPR 18,000",
        change: "+4.2%",
        shows: 4,
        avgPerShow: "NPR 4,500",
        occupancy: "52%",
      },
    ],
  },
  weekend: {
    dateRange: "Mar 8-9, 2025",
    metrics: {
      boxOffice: { value: "NPR 260,000", change: 8.5 },
      ticketsSold: { value: "2,030", change: 7.2 },
      avgTicketPrice: { value: "NPR 128.10", change: 1.2 },
      avgOccupancy: { value: "72%", change: 5 },
    },
    revenueData: [
      { day: "Mar 8", value: 135000 },
      { day: "Mar 9", value: 125000 },
    ],
    ticketData: [
      { day: "Mar 8", value: 1050 },
      { day: "Mar 9", value: 980 },
    ],
    occupancyData: [
      { name: "Occupied", value: 72 },
      { name: "Vacant", value: 28 },
    ],
    movieData: [
      {
        movie: "Himalayan Heights",
        gross: "NPR 75,000",
        change: "+9.2%",
        shows: 6,
        avgPerShow: "NPR 12,500",
        occupancy: "80%",
      },
      {
        movie: "Kathmandu Dreams",
        gross: "NPR 65,000",
        change: "+6.5%",
        shows: 5,
        avgPerShow: "NPR 13,000",
        occupancy: "75%",
      },
      {
        movie: "Mountain Melody",
        gross: "NPR 52,000",
        change: "+10.5%",
        shows: 4,
        avgPerShow: "NPR 13,000",
        occupancy: "72%",
      },
      {
        movie: "Village Tales",
        gross: "NPR 40,000",
        change: "+7.8%",
        shows: 3,
        avgPerShow: "NPR 13,333",
        occupancy: "68%",
      },
      {
        movie: "River's Journey",
        gross: "NPR 28,000",
        change: "+12.5%",
        shows: 2,
        avgPerShow: "NPR 14,000",
        occupancy: "65%",
      },
    ],
  },
  weekly: {
    dateRange: "Mar 7-13, 2025",
    metrics: {
      boxOffice: { value: "NPR 845,000", change: 5.2 },
      ticketsSold: { value: "6,610", change: 4.5 },
      avgTicketPrice: { value: "NPR 127.80", change: 0.8 },
      avgOccupancy: { value: "55%", change: 2.2 },
    },
    revenueData: [
      { day: "Week 10", value: 805000 },
      { day: "Week 11", value: 845000 },
    ],
    ticketData: [
      { day: "Week 10", value: 6320 },
      { day: "Week 11", value: 6610 },
    ],
    occupancyData: [
      { name: "Occupied", value: 55 },
      { name: "Vacant", value: 45 },
    ],
    movieData: [
      {
        movie: "Himalayan Heights",
        gross: "NPR 240,000",
        change: "+6.5%",
        shows: 35,
        avgPerShow: "NPR 6,857",
        occupancy: "62%",
      },
      {
        movie: "Kathmandu Dreams",
        gross: "NPR 210,000",
        change: "+3.8%",
        shows: 30,
        avgPerShow: "NPR 7,000",
        occupancy: "58%",
      },
      {
        movie: "Mountain Melody",
        gross: "NPR 170,000",
        change: "+5.5%",
        shows: 25,
        avgPerShow: "NPR 6,800",
        occupancy: "55%",
      },
      {
        movie: "Village Tales",
        gross: "NPR 135,000",
        change: "+1.2%",
        shows: 20,
        avgPerShow: "NPR 6,750",
        occupancy: "52%",
      },
      {
        movie: "River's Journey",
        gross: "NPR 90,000",
        change: "+8.5%",
        shows: 15,
        avgPerShow: "NPR 6,000",
        occupancy: "48%",
      },
    ],
  },
  monthly: {
    dateRange: "Mar 2025",
    metrics: {
      boxOffice: { value: "NPR 3,400,000", change: 10.2 },
      ticketsSold: { value: "26,600", change: 8.5 },
      avgTicketPrice: { value: "NPR 127.80", change: 1.5 },
      avgOccupancy: { value: "52%", change: 3 },
    },
    revenueData: [
      { day: "Jan", value: 2950000 },
      { day: "Feb", value: 3100000 },
      { day: "Mar", value: 3400000 },
    ],
    ticketData: [
      { day: "Jan", value: 23200 },
      { day: "Feb", value: 24500 },
      { day: "Mar", value: 26600 },
    ],
    occupancyData: [
      { name: "Occupied", value: 52 },
      { name: "Vacant", value: 48 },
    ],
    movieData: [
      {
        movie: "Himalayan Heights",
        gross: "NPR 950,000",
        change: "+12.5%",
        shows: 140,
        avgPerShow: "NPR 6,786",
        occupancy: "60%",
      },
      {
        movie: "Kathmandu Dreams",
        gross: "NPR 780,000",
        change: "+8.5%",
        shows: 120,
        avgPerShow: "NPR 6,500",
        occupancy: "55%",
      },
      {
        movie: "Mountain Melody",
        gross: "NPR 650,000",
        change: "+10.2%",
        shows: 100,
        avgPerShow: "NPR 6,500",
        occupancy: "52%",
      },
      {
        movie: "Village Tales",
        gross: "NPR 580,000",
        change: "+6.8%",
        shows: 90,
        avgPerShow: "NPR 6,444",
        occupancy: "48%",
      },
      {
        movie: "River's Journey",
        gross: "NPR 440,000",
        change: "+15.2%",
        shows: 70,
        avgPerShow: "NPR 6,286",
        occupancy: "45%",
      },
    ],
  },
};

// Create the complete data structure with proper data for each theater
export const theaterData = {
  qfx: createTheaterData(qfxBaseData),
  fcube: createTheaterData(fcubeBaseData),
  big: createTheaterData(bigBaseData),
};

// Function to generate slightly different data for refresh simulation
export const refreshData = (data) => {
  // Helper to add random variation to numbers
  const addVariation = (value, percentage = 5) => {
    const variation = ((Math.random() * 2 - 1) * percentage) / 100;
    return value * (1 + variation);
  };

  // Deep clone and modify the data
  return {
    ...data,
    metrics: {
      boxOffice: {
        value: `NPR ${Math.floor(
          addVariation(
            Number.parseInt(data.metrics.boxOffice.value.replace(/[^0-9]/g, ""))
          )
        ).toLocaleString()}`,
        change: +addVariation(data.metrics.boxOffice.change).toFixed(1),
      },
      ticketsSold: {
        value: Math.floor(
          addVariation(
            Number.parseInt(
              data.metrics.ticketsSold.value.replace(/[^0-9]/g, "")
            )
          )
        ).toLocaleString(),
        change: +addVariation(data.metrics.ticketsSold.change).toFixed(1),
      },
      avgTicketPrice: {
        value: `NPR ${addVariation(
          Number.parseFloat(
            data.metrics.avgTicketPrice.value.replace(/[^0-9.]/g, "")
          ),
          2
        ).toFixed(2)}`,
        change: +addVariation(data.metrics.avgTicketPrice.change, 2).toFixed(1),
      },
      avgOccupancy: {
        value: `${Math.min(
          100,
          Math.floor(
            addVariation(
              Number.parseInt(
                data.metrics.avgOccupancy.value.replace(/[^0-9]/g, "")
              ),
              3
            )
          )
        )}%`,
        change: +addVariation(data.metrics.avgOccupancy.change, 3).toFixed(1),
      },
    },
    revenueData: data.revenueData.map((item) => ({
      ...item,
      value: Math.floor(addVariation(item.value)),
    })),
    ticketData: data.ticketData.map((item) => ({
      ...item,
      value: Math.floor(addVariation(item.value)),
    })),
    occupancyData: [
      {
        name: "Occupied",
        value: Math.min(
          100,
          Math.floor(addVariation(data.occupancyData[0].value, 3))
        ),
      },
      {
        name: "Vacant",
        value:
          100 -
          Math.min(
            100,
            Math.floor(addVariation(data.occupancyData[0].value, 3))
          ),
      },
    ],
    movieData: data.movieData.map((movie) => {
      const changeValue = Number.parseFloat(
        movie.change.replace(/[^0-9.]/g, "")
      );
      const isPositive = movie.change.startsWith("+");
      const newChangeValue = addVariation(changeValue, 10);

      return {
        ...movie,
        gross: `NPR ${Math.floor(
          addVariation(Number.parseInt(movie.gross.replace(/[^0-9]/g, "")))
        ).toLocaleString()}`,
        change: (isPositive ? "+" : "-") + newChangeValue.toFixed(1) + "%",
        shows: Math.max(1, Math.floor(addVariation(movie.shows, 2))),
        avgPerShow: `NPR ${Math.floor(
          addVariation(Number.parseInt(movie.avgPerShow.replace(/[^0-9]/g, "")))
        ).toLocaleString()}`,
        occupancy: `${Math.min(
          100,
          Math.floor(
            addVariation(
              Number.parseInt(movie.occupancy.replace(/[^0-9]/g, "")),
              3
            )
          )
        )}%`,
      };
    }),
  };
};
