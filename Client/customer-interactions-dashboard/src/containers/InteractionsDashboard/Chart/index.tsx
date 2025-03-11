import { LineChart, Line, XAxis, YAxis, Tooltip, ResponsiveContainer } from "recharts";
import { useChart } from "./hooks/useChart";

const Chart: React.FC = () => {
  const chartData = useChart();

  return (
    <ResponsiveContainer width="100%" height={300}>
      <LineChart data={chartData}>
        <XAxis dataKey="date" />
        <YAxis allowDecimals={false} />
        <Tooltip 
          content={({ payload }) => {
            if (!payload || payload.length === 0) return null;
            const { date, notes } = payload[0].payload;
            return (
              <div className="bg-white p-2 shadow-md rounded text-sm">
                <p className="font-bold">{date}</p>
                {notes.length > 0 ? (
                  <ul className="list-disc pl-4">
                    {notes.map((note: string, index: number) => <li key={index}>{note}</li>)}
                  </ul>
                ) : (
                  <p>No notes available</p>
                )}
              </div>
            );
          }}
        />
        <Line type="monotone" dataKey="count" stroke="#8884d8" strokeWidth={2} />
      </LineChart>
    </ResponsiveContainer>
  );
};

export default Chart;