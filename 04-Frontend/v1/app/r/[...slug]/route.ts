import { NextResponse } from "next/server";

export async function GET(
  req: Request,
  ctx: { params: Promise<{ slug?: string[] }> }
) {
  const { slug } = await ctx.params;

  const path = Array.isArray(slug) ? slug.join("/") : "";

  return NextResponse.json({
    name: "TEST DE RESPUESTA",
    ok: true,
    path,
  });
}
