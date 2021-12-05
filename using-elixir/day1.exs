{:ok, data} = File.read("./data/D_1/input.txt")

count_increases = fn set ->
  set
    |> Enum.chunk_every(2, 1, :discard)
    |> Enum.filter(fn [a, b] -> a < b end)
    |> Enum.count
end

p1_set =
  to_string(data)
  |> String.split("\r\n", trim: true)
  |> Enum.map(&Integer.parse/1)
  |> Enum.map(fn {i, _} -> i end)

p2_set =
  p1_set
    |> Enum.chunk_every(3, 1)
    |> Enum.map(&Enum.sum/1)

IO.puts(count_increases.(p1_set))
IO.puts(count_increases.(p2_set))
