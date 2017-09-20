﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;

namespace ElmSharp
{
    /// <summary>
    /// This group provides functions for image objects.
    /// </summary>
    public class EvasImage : EvasObject
    {
        EvasObject _source = null;
        IntPtr _handle = IntPtr.Zero;

        /// <summary>
        /// Creates and initializes a new instance of EvasImage class.
        /// </summary>
        /// <param name="parent">The parent is a given container which will be attached by EvasImage as a child. It's <see cref="EvasObject"/> type.</param>
        public EvasImage(EvasObject parent) : base(parent)
        {
        }

        internal EvasImage(EvasObject parent, IntPtr handle) : base()
        {
            _handle = handle;
            Realize(parent);
        }

        /// <summary>
        /// Sets or gets the source file from where an image object must fetch the real image data
        /// </summary>
        public string File
        {
            get
            {
                string file, key;
                Interop.Evas.evas_object_image_file_get(RealHandle, out file, out key);
                return file;
            }
            set
            {
                Interop.Evas.evas_object_image_file_set(RealHandle, value, null);
            }
        }

        /// <summary>
        /// Sets or gets the source object to be visible.
        /// </summary>
        public bool IsSourceVisible
        {
            get
            {
                return Interop.Evas.evas_object_image_source_visible_get(RealHandle);
            }
            set
            {
                Interop.Evas.evas_object_image_source_visible_set(RealHandle, value);
            }
        }

        /// <summary>
        /// Sets or gets whether an object is clipped by source object's clipper.
        /// </summary>
        public bool IsSourceClipped
        {
            get
            {
                return Interop.Evas.evas_object_image_source_clip_get(RealHandle);
            }
            set
            {
                Interop.Evas.evas_object_image_source_clip_set(RealHandle, value);
            }
        }

        /// <summary>
        /// Sets or gets if the center part of the given image object (not the border) should be drawn.
        /// </summary>
        /// <remarks>
        /// When rendering, the image may be scaled to fit the size of the image object.
        /// This function sets if the center part of the scaled image is to be drawn or left completely blank, or forced to be solid.
        /// Very useful for frames and decorations.
        /// </remarks>
        public ImageBorderFillMode BorderCenterFillMode
        {
            get
            {
                return (ImageBorderFillMode)Interop.Evas.evas_object_image_border_center_fill_get(RealHandle);
            }
            set
            {
                Interop.Evas.evas_object_image_border_center_fill_set(RealHandle, (int)value);
            }
        }

        /// <summary>
        /// Sets or gets whether the image object's fill property should track the object's size.
        /// </summary>
        public bool IsFilled
        {
            get
            {
                return Interop.Evas.evas_object_image_filled_get(RealHandle);
            }
            set
            {
                Interop.Evas.evas_object_image_filled_set(RealHandle, value);
            }
        }

        /// <summary>
        /// Sets or gets the scaling factor (multiplier) for the borders of an image object.
        /// </summary>
        public double BorderScale
        {
            get
            {
                return Interop.Evas.evas_object_image_border_scale_get(RealHandle);
            }
            set
            {
                Interop.Evas.evas_object_image_border_scale_set(RealHandle, value);
            }
        }

        /// <summary>
        /// Sets or gets the size of the given image object.
        /// </summary>
        public Size Size
        {
            get
            {
                int w, h;
                Interop.Evas.evas_object_image_size_get(RealHandle, out w, out h);
                return new Size(w, h);
            }
            set
            {
                Interop.Evas.evas_object_image_size_set(RealHandle, value.Width, value.Height);
            }
        }

        /// <summary>
        /// Gets the row stride of the given image object.
        /// </summary>
        public int Stride
        {
            get
            {
                return Interop.Evas.evas_object_image_stride_get(RealHandle);
            }
        }

        /// <summary>
        /// Sets or gets whether alpha channel data is being used on the given image object.
        /// </summary>
        public bool IsOpaque
        {
            get
            {
                return !Interop.Evas.evas_object_image_alpha_get(RealHandle);
            }
            set
            {
                Interop.Evas.evas_object_image_alpha_set(RealHandle, !value);

            }
        }

        /// <summary>
        /// Sets or gets whether to use high-quality image scaling algorithm on the given image object.
        /// </summary>
        public bool IsSmoothScaled
        {
            get
            {
                return Interop.Evas.evas_object_image_smooth_scale_get(RealHandle);
            }
            set
            {
                Interop.Evas.evas_object_image_smooth_scale_set(RealHandle, value);
            }
        }

        /// <summary>
        /// Sets how to fill an image object's drawing rectangle given the (real) image bound to it.
        /// </summary>
        /// <param name="geometry"></param>
        public void SetFill(Rect geometry)
        {
            Interop.Evas.evas_object_image_fill_set(RealHandle, geometry.X, geometry.Y, geometry.Width, geometry.Height);
        }

        /// <summary>
        /// Sets the source file from where an image object must fetch the real image data (it may be an Eet file, besides pure image ones).
        /// </summary>
        /// <param name="file">The image file path</param>
        /// <param name="key">The image key in file (if its an Eet one), otherwise set null</param>
        public void SetFile(string file, string key)
        {
            Interop.Evas.evas_object_image_file_set(RealHandle, file, key);
        }

        /// <summary>
        /// Sets the data for an image from memory to be loaded.
        /// </summary>
        /// <param name="stream">memory stream</param>
        public void SetStream(Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");

            MemoryStream memstream = new MemoryStream();
            stream.CopyTo(memstream);
            unsafe
            {
                byte[] dataArr = memstream.ToArray();
                fixed (byte* data = &dataArr[0])
                {
                    Interop.Evas.evas_object_image_memfile_set(RealHandle, data, dataArr.Length, IntPtr.Zero, IntPtr.Zero);
                }
            }
        }

        /// <summary>
        /// Sets the source object on an image object to used as a proxy.
        /// </summary>
        /// <param name="source">The proxy (image) object</param>
        /// <returns>true if the source object is set successfully, ortherwise false on error</returns>
        public bool SetSource(EvasObject source)
        {
            bool result = false;
            _source = source;
            result = Interop.Evas.evas_object_image_source_set(RealHandle, IntPtr.Zero);
            if (source != null)
                result = result && Interop.Evas.evas_object_image_source_set(RealHandle, source.Handle);
            return result;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public void SetNativeSurface(IntPtr surface)
        {
            Interop.Evas.evas_object_image_native_surface_set(RealHandle, surface);
        }

        /// <summary>
        /// Sets the dimensions for an image object's border, a region which is not scaled together with its center ever.
        /// </summary>
        /// <param name="left">The border's left width</param>
        /// <param name="right">The border's right width</param>
        /// <param name="top">The border's top width</param>
        /// <param name="bottom">The border's bottom width</param>
        public void SetBorder(int left, int right, int top, int bottom)
        {
            Interop.Evas.evas_object_image_border_set(RealHandle, left, right, top, bottom);
        }

        /// <summary>
        /// Sets the content at a part of a given container widget.
        /// </summary>
        /// <param name="parent">The parent is a given container which will be attached by Image as a child. It's <see cref="EvasObject"/> type.</param>
        /// <returns>The new object, otherwise null if it cannot be created</returns>
        protected override IntPtr CreateHandle(EvasObject parent)
        {
            return _handle != IntPtr.Zero ? _handle : Interop.Evas.evas_object_image_add(Interop.Evas.evas_object_evas_get(parent.Handle));
        }
    }
}