import { Card, CardContent } from "@/components/ui/card";
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from "@/components/ui/table";

export function MoviePerformanceTable({ data }) {
  return (
    <Card>
      <CardContent className="pt-6">
        <h3 className="text-lg font-semibold mb-4">Movie Performance</h3>
        <Table>
          <TableHeader>
            <TableRow>
              <TableHead>Movie</TableHead>
              <TableHead>Gross</TableHead>
              <TableHead>% Change</TableHead>
              <TableHead>Shows</TableHead>
              <TableHead>Avg. Per Show</TableHead>
              <TableHead>Occupancy</TableHead>
            </TableRow>
          </TableHeader>
          <TableBody>
            {data.map((movie, index) => (
              <TableRow key={index}>
                <TableCell className="font-medium">{movie.movie}</TableCell>
                <TableCell>{movie.gross}</TableCell>
                <TableCell
                  className={
                    Number.parseFloat(movie.change) >= 0
                      ? "text-green-500"
                      : "text-red-500"
                  }
                >
                  {movie.change}
                </TableCell>
                <TableCell>{movie.shows}</TableCell>
                <TableCell>{movie.avgPerShow}</TableCell>
                <TableCell>{movie.occupancy}</TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </CardContent>
    </Card>
  );
}
