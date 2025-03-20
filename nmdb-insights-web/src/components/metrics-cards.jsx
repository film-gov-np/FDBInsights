import { Card, CardContent } from "@/components/ui/card";

export function MetricsCards({ metrics }) {
  return (
    <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-4 mb-8">
      <Card>
        <CardContent className="pt-6">
          <div className="text-sm text-muted-foreground mb-1">
            Total Box Office
          </div>
          <div className="text-2xl font-bold mb-1">
            {metrics.boxOffice.value}
          </div>
          <div
            className={`text-sm ${
              metrics.boxOffice.change >= 0 ? "text-green-500" : "text-red-500"
            }`}
          >
            {metrics.boxOffice.change >= 0 ? "+" : ""}
            {metrics.boxOffice.change}% from yesterday
          </div>
        </CardContent>
      </Card>
      <Card>
        <CardContent className="pt-6">
          <div className="text-sm text-muted-foreground mb-1">Tickets Sold</div>
          <div className="text-2xl font-bold mb-1">
            {metrics.ticketsSold.value}
          </div>
          <div
            className={`text-sm ${
              metrics.ticketsSold.change >= 0
                ? "text-green-500"
                : "text-red-500"
            }`}
          >
            {metrics.ticketsSold.change >= 0 ? "+" : ""}
            {metrics.ticketsSold.change}% from yesterday
          </div>
        </CardContent>
      </Card>
      <Card>
        <CardContent className="pt-6">
          <div className="text-sm text-muted-foreground mb-1">
            Average Ticket Price
          </div>
          <div className="text-2xl font-bold mb-1">
            {metrics.avgTicketPrice.value}
          </div>
          <div
            className={`text-sm ${
              metrics.avgTicketPrice.change >= 0
                ? "text-green-500"
                : "text-red-500"
            }`}
          >
            {metrics.avgTicketPrice.change >= 0 ? "+" : ""}
            {metrics.avgTicketPrice.change}% from yesterday
          </div>
        </CardContent>
      </Card>
      <Card>
        <CardContent className="pt-6">
          <div className="text-sm text-muted-foreground mb-1">
            Average Occupancy
          </div>
          <div className="text-2xl font-bold mb-1">
            {metrics.avgOccupancy.value}
          </div>
          <div
            className={`text-sm ${
              metrics.avgOccupancy.change >= 0
                ? "text-green-500"
                : "text-red-500"
            }`}
          >
            {metrics.avgOccupancy.change >= 0 ? "+" : ""}
            {metrics.avgOccupancy.change}% from yesterday
          </div>
        </CardContent>
      </Card>
    </div>
  );
}
