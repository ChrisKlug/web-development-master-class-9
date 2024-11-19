export async function getMe(client: typeof fetch): Promise<string | undefined> {
	try {
		const response = await client(`/api/me`);
		const name = await response.text();
		return name.replace(/["]+/g, '');
	} catch {
		return undefined;
	}
}
