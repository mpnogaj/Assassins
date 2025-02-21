export const insertBetween = <T, Y>(array: T[], element: Y) => {
	return array.flatMap(x => [element, x]).slice(1);
};

export const dateToString = (date: Date) => {
	return `${date.getDate()}.${date.getMonth() + 1}.${date.getFullYear()} ${date.getHours()}:${date.getMinutes()}:${date.getSeconds()}`;
};
