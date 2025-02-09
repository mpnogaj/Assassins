import { useCallback, useEffect, useState } from 'react';

export function useDataFetch<T>(dataFetcher: () => Promise<T | undefined>) {
	const [data, setData] = useState<T | undefined>(undefined);
	const [isLoading, setIsLoading] = useState<boolean>(true);
	const [isError, setIsError] = useState<boolean>(false);

	const fetchData = useCallback(() => {
		setIsLoading(true);
		setIsError(false);

		dataFetcher()
			.then(fetchedData => {
				if (fetchedData !== undefined) {
					setData(fetchedData);
					setIsError(false);
				} else {
					setIsError(true);
				}
			})
			.catch(() => setIsError(true))
			.finally(() => setIsLoading(false));
	}, [dataFetcher]);

	useEffect(() => {
		fetchData();
	}, [fetchData]);

	return { data, isLoading, isError, refetch: fetchData };
}
