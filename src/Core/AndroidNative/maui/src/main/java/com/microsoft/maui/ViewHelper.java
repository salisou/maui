package com.microsoft.maui;

import android.graphics.Paint;
import android.graphics.Typeface;
import android.view.View;
import android.view.ViewGroup;
import android.view.ViewParent;
import android.widget.TextView;

public class ViewHelper {
    public static void requestLayoutIfNeeded(View view)
    {
        if (!view.isInLayout())
            view.requestLayout();
    }

    public static void removeFromParent(View view)
    {
        ViewParent parent = view.getParent();
        if (parent == null)
            return;
        ((ViewGroup)parent).removeView(view);
    }

    public static void setPivotXIfNeeded(View view, float pivotX)
    {
        if (view.getPivotX() != pivotX)
            view.setPivotX(pivotX);
    }

    public static void setPivotYIfNeeded(View view, float pivotY)
    {
        if (view.getPivotY() != pivotY)
            view.setPivotY(pivotY);
    }

    public static void setView(
        View view,
        int automationTagId,
        String automationId,
        int visibility,
        int layoutDirection,
        int minimumHeight,
        int minimumWidth,
        boolean enabled,
        float alpha,
        float translationX,
        float translationY,
        float scaleX,
        float scaleY,
        float rotation,
        float rotationX,
        float rotationY,
        float pivotX,
        float pivotY)
    {
        requestLayoutIfNeeded(view);
        view.setTag(automationTagId, automationId);
        view.setVisibility(visibility);
        view.setLayoutDirection(layoutDirection);
        view.setMinimumHeight(minimumHeight);
        view.setMinimumWidth(minimumWidth);
        view.setEnabled(enabled);
        view.setAlpha(alpha);
        view.setTranslationX(translationX);
        view.setTranslationY(translationY);
        view.setScaleX(scaleX);
        view.setScaleY(scaleY);
        view.setRotation(rotation);
        view.setRotationX(rotationX);
        view.setRotationY(rotationY);
        setPivotXIfNeeded(view, pivotX);
        setPivotYIfNeeded(view, pivotY);
    }

    public static void setPaintFlags(TextView textView, boolean strikeThrough, boolean underline)
    {
        int paintFlags = textView.getPaintFlags();
        if (strikeThrough)
            paintFlags |= Paint.STRIKE_THRU_TEXT_FLAG;
        else
            paintFlags &= ~Paint.STRIKE_THRU_TEXT_FLAG;
        if (underline)
            paintFlags |= Paint.UNDERLINE_TEXT_FLAG;
        else
            paintFlags &= ~Paint.UNDERLINE_TEXT_FLAG;
        textView.setPaintFlags(paintFlags);
    }

    public static void setTextView(
        TextView textView,
        int automationTagId,
        String automationId,
        int visibility,
        int layoutDirection,
        int minimumHeight,
        int minimumWidth,
        boolean enabled,
        float alpha,
        float translationX,
        float translationY,
        float scaleX,
        float scaleY,
        float rotation,
        float rotationX,
        float rotationY,
        float pivotX,
        float pivotY,
        float letterSpacing,
        Typeface typeface,
        int textSizeUnit,
        float textSize,
        int textAlignment,
        int gravity,
        boolean singleLine,
        int maxLines,
        int lineSpacing,
        int paddingLeft,
        int paddingTop,
        int paddingRight,
        int paddingBottom,
        CharSequence text,
        int textColor,
        boolean strikeThrough,
        boolean underline)
    {
        setView(textView, automationTagId, automationId, visibility, layoutDirection, minimumHeight, minimumWidth, enabled, alpha, translationX, translationY, scaleX, scaleY, rotation, rotationX, rotationY, pivotX, pivotY);

        textView.setLetterSpacing(letterSpacing);
        textView.setTypeface(typeface);
        textView.setTextSize(textSizeUnit, textSize);
        textView.setTextAlignment(textAlignment);
        textView.setGravity(gravity);
        textView.setSingleLine(singleLine);
        textView.setMaxLines(maxLines);
        textView.setLineSpacing(0, lineSpacing);
        textView.setPadding(paddingLeft, paddingTop, paddingRight, paddingBottom);
        textView.setText(text);
        textView.setTextColor(textColor);
        setPaintFlags(textView, strikeThrough, underline);
    }
}